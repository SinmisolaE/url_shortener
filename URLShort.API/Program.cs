using URLShort.API.Interfaces;
using URLShort.API.Service;
using URLShort.Core;
using URLShort.Infrastructure.Data;
using URLShort.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using URLShort.Infrastructure.Middleware;
using AspNetCoreRateLimit;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Log/log.txt")
    .CreateLogger();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

// builder.Services.AddSerilog();  this replaces built-in logging totally

builder.Logging.AddSerilog();

/*

built in logging


builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
*/

//Adding health check
builder.Services.AddHealthChecks();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<ShortenUrlDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
    b => b.MigrationsAssembly("URLShort.Infrastructure")) // Tell ef wehere migrations are
);

//register url  configuration
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped<IUrlRepository, UrlRespository>();
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddSingleton<IEncode, Encode>();


//implement health check for db
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ShortenUrlDbContext>(
        name: "url-shortenr-db",
        tags: new[] { "ready" }
);

builder.Services.AddMemoryCache();
builder.Services.AddInMemoryRateLimiting();


// the two below for setting an example

builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();   //Add middleware

app.UseHttpsRedirection();

app.UseIpRateLimiting();  // middleware to handle rate limiting

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
