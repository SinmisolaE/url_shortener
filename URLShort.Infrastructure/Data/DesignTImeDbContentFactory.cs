using System;
// Infrastructure/Data/DesignTimeDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace URLShort.Infrastructure.Data;

// This class is ONLY used by the 'dotnet ef' commands
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ShortenUrlDbContext>
{
    public ShortenUrlDbContext CreateDbContext(string[] args)
    {
        // 1. Build configuration to read from appsettings.json
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../URLShort.API"))             .AddJsonFile("appsettings.json")
            .Build();

        // 2. Get the connection string from the configuration
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Could not find connection string named 'DefaultConnection'");
        }

        // 3. Create the DbContextOptionsBuilder specifically for MySQL
        var optionsBuilder = new DbContextOptionsBuilder<ShortenUrlDbContext>();
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    b => b.MigrationsAssembly("URLShort.Infrastructure"));
        

        // 4. Return a new instance of the DbContext with the configured options
        return new ShortenUrlDbContext(optionsBuilder.Options);
    }
}
