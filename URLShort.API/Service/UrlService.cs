using System;
using Microsoft.Extensions.Options;
using URLShort.API.DTO;
using URLShort.API.Interfaces;
using URLShort.Core;

namespace URLShort.API.Service {

    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _repository;

        private readonly IEncode _encode;
        private const int MaxLimit = 2000;


        //inject appsettings url {localhost:7070} to my service
        private readonly AppSettings _appSettings;

        public UrlService(IUrlRepository repository, IEncode encode, IOptions<AppSettings> appSettings)
        {
            _repository = repository;
            _encode = encode;
            _appSettings = appSettings.Value;
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
            var url = await _repository.GetUrlByShortUrlAsync(shortCode);

            if (url == null)
            {
                throw new Exception("Url not found");
            }

            return $"{url.LongURL}";
        }

        public async Task<string> AddUrlAsync(UrlDTO urlDTO)
        {
            System.Console.WriteLine($"yeee:  {_appSettings.BaseUrl}");

            // Handle likely errors
            if (string.IsNullOrEmpty(urlDTO.LongURL))
            {
                throw new ArgumentException("Url caannot be empty");
            }

            if (urlDTO.LongURL.Length > MaxLimit)
            {
                throw new Exception("Url too long");
            }

            var url = new ShortenUrl(urlDTO.LongURL);

            var confirm_url = await _repository.GetUrlByLongUrlAsync(url.LongURL);

            if (confirm_url != null)
            {
                return $"{_appSettings.BaseUrl}/{confirm_url.ShortURL}";
            }

            var add_url = await _repository.AddUrlAsync(url);

            if (add_url == null)
            {
                throw new Exception("Url not added");
            }

            var shortCode = _encode.EncodeValue(add_url.Id);

            add_url.ShortURL = shortCode;

            await _repository.SaveChangesAsync();

            return $"{_appSettings.BaseUrl}/{add_url.ShortURL}";
        }

    }
}
