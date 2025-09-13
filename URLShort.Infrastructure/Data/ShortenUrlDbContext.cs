using System;
using Microsoft.EntityFrameworkCore;
using URLShort.Core;

namespace URLShort.Infrastructure.Data;

public class ShortenUrlDbContext : DbContext
{
    public ShortenUrlDbContext(DbContextOptions<ShortenUrlDbContext> options) : base(options)
    {
    }

    public DbSet<ShortenUrl> ShortenUrls { get; set; }
}
