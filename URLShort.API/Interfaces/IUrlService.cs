using System;
using URLShort.API.DTO;
using URLShort.Core;

namespace URLShort.API.Interfaces
{

    public interface IUrlService
    {
        Task<ShortenUrl> GetUrlByIdAsync(int id);
        Task<string> GetUrlByShortUrlAsync(string shortCodes);
        //Task<ShortenUrl> GetUrlByLongUrlAsync(string longUrl);
        Task<string> AddUrlAsync(UrlDTO urlDTO);

    }
}
