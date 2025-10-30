using System;
using URLShort.Core;

namespace URLShort.Core.Interfaces.ServiceInterfaces
{

    public interface IUrlService
    {
        Task<ShortenUrl> GetUrlByIdAsync(int id);
        Task<string> GetUrlByShortUrlAsync(string shortCodes);
//        Task<ShortenUrl> GetUrlByLongUrlAsync(string longUrl);
        Task<string> AddUrlAsync(string longUrl);

    }
}
