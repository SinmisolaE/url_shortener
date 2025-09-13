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

            var url = new ShortenUrl(urlDTO.LongURL);


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
