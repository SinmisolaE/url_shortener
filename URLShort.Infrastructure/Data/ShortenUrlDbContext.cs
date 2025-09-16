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

    //handle indexing and uniqueness for longurl and shorturl
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenUrl>()
        .HasIndex(u => u.ShortURL)
        .IsUnique();

        modelBuilder.Entity<ShortenUrl>()
        .HasIndex(u => u.LongURL)
        .IsUnique();
    }
}
