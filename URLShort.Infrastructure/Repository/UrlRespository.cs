using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using URLShort.Core;
using URLShort.Infrastructure.Data;

namespace URLShort.Infrastructure.Repository;

public class UrlRespository : IUrlRepository
{
    private readonly ShortenUrlDbContext _context;
    private readonly ILogger<IUrlRepository> _logger;

    public UrlRespository(ShortenUrlDbContext context, ILogger<IUrlRepository> logger)
    {
        _context = context;
        _logger = logger;
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

    public async Task<ShortenUrl> AddUrlAsync(ShortenUrl longUrl)
    {

        try
        {
            _logger.LogInformation("trying to add long url at repository level");
            await _context.ShortenUrls.AddAsync(longUrl);

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            System.Console.WriteLine("oooppsss");

            _logger.LogError($"Error:  {e.Message}");

            throw;

        }
        return longUrl;
    }

    public async Task SaveChangesAsync()
    {
        try
        {

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError($"Error: {e.Message}");
            throw;
        }
        
    }
}
