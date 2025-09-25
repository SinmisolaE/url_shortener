using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MySqlConnector;
using URLShort.API.DTO;
using URLShort.API.Interfaces;
using URLShort.Core;
using URLShort.Core.Exceptions;

namespace URLShort.API.Service {

    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _repository;
        private readonly IEncode _encode;
        private const int MaxLimit = 2000;
        private ILogger<UrlService> _logger;
        private readonly AppSettings _appSettings; //inject appsettings url {localhost:7070} to my service
        private readonly IDistributedCache _distributedCache;

        public UrlService(IUrlRepository repository, IEncode encode, IOptions<AppSettings> appSettings, ILogger<UrlService> logger, IDistributedCache distributedCache)
        {
            _repository = repository;
            _encode = encode;
            _appSettings = appSettings.Value;
            _logger = logger;
            _distributedCache = distributedCache;
        }

        public async Task<ShortenUrl> GetUrlByIdAsync(int id)
        {
            var url = await _repository.GetUrlByIdAsync(id);

            if (url == null)
            {
                throw new Exception("Url not found");
            }

            return url;
        }

        public async Task<string> GetUrlByShortUrlAsync(string shortCode)
        {
            string cache_key = $"shortCode_{shortCode}";

            //check cache
            string longUrl = await _distributedCache.GetStringAsync(cache_key);

            _logger.LogInformation("Checking cache");

            if (string.IsNullOrEmpty(longUrl))
            {
                _logger.LogInformation("Data not in cache! Checking database");
                var url = await _repository.GetUrlByShortUrlAsync(shortCode);

                if (url == null)
                {
                    _logger.LogError("URL not found");
                    throw new Exception("Url not found");
                }

                _logger.LogInformation("Storing data to cache");
                await _distributedCache.SetStringAsync(cache_key, url.LongURL);

                return $"{url.LongURL}";
            }

            return longUrl;  
        }

        public async Task<string> AddUrlAsync(UrlDTO urlDTO)
        {
            System.Console.WriteLine($"yeee:  {_appSettings.BaseUrl}");

            // Validate input
            if (string.IsNullOrEmpty(urlDTO.LongURL))
            {
                throw new ArgumentException("Url caannot be empty");
            }

            if (urlDTO.LongURL.Length > MaxLimit)
            {
                throw new UrlTooLongException(MaxLimit, urlDTO.LongURL.Length);
            }

            // Check if url already exists
            var confirm_url = await _repository.GetUrlByLongUrlAsync(urlDTO.LongURL);

            if (confirm_url != null)
            {
                return $"{_appSettings.BaseUrl}/{confirm_url.ShortURL}";
            }

            var url = new ShortenUrl(urlDTO.LongURL);

            try
            {
                var add_url = await _repository.AddUrlAsync(url);

                var shortCode = _encode.EncodeValue(add_url.Id);

                add_url.ShortURL = shortCode;

                await _repository.SaveChangesAsync();

                return $"{_appSettings.BaseUrl}/{add_url.ShortURL}";
            }
            catch (DbUpdateException e) when (IsUniqueConstraintViolation(e))
            {
                var raceConditionUrl = await _repository.GetUrlByLongUrlAsync(urlDTO.LongURL);

                if (raceConditionUrl != null)
                {
                    return $"{_appSettings.BaseUrl}/{raceConditionUrl.ShortURL}";
                }

                _logger.LogError("Failed to create short url");
                throw new Exception("Error: Failed to create short url");
            }

        }

        // validate the exception
        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            return ex.InnerException is MySqlException mySql &&
                mySql.ErrorCode == MySqlErrorCode.DuplicateKeyEntry;
        }

    }

    
} 
