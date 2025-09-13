

namespace URLShort.Core
{

    public interface IUrlRepository
    {
        Task<ShortenUrl?> GetUrlByIdAsync(int id);
        Task<ShortenUrl> AddUrlAsync(ShortenUrl shorten);

        Task<ShortenUrl?> GetUrlByShortUrlAsync(string shortenUrl);
        Task<ShortenUrl?> GetUrlByLongUrlAsync(string longUrl);

        Task SaveChangesAsync();

    }
}