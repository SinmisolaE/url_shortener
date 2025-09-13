using System;
using Microsoft.EntityFrameworkCore;
using URLShort.Core;
using URLShort.Infrastructure.Data;

namespace URLShort.Infrastructure.Repository;

public class UrlRespository : IUrlRepository
{
    private readonly ShortenUrlDbContext _context;

    public UrlRespository(ShortenUrlDbContext context)
    {
        _context = context;
    }


    public async Task<ShortenUrl?> GetUrlByIdAsync(int id)
    {
        return await _context.ShortenUrls.FindAsync(id);
    }

    public async Task<ShortenUrl?> GetUrlByShortUrlAsync(string shortenUrl)
    {
        return await _context.ShortenUrls.FirstOrDefaultAsync(x => x.ShortURL == shortenUrl);
    }

    public async Task<ShortenUrl?> GetUrlByLongUrlAsync(string longUrl)
    {
        return await _context.ShortenUrls.FirstOrDefaultAsync(x => x.LongURL == longUrl);
    }

    public async Task<ShortenUrl> AddUrlAsync(ShortenUrl shorten)
    {
        await _context.ShortenUrls.AddAsync(shorten);
        try
        {

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                Console.WriteLine("errororr\n\n err: " + e.InnerException.Message);
            }
        }
        return shorten;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
        
    }
}
