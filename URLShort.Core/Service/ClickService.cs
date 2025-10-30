using System;
using Microsoft.Extensions.Logging;
using URLShort.Core.Interfaces.ServiceInterfaces;

namespace URLShort.Core.Service;

public class ClickService : IClickService
{
    private readonly IUrlRepository _repository;

    private readonly ILogger<ClickService> _logger;

    public ClickService(IUrlRepository repository, ILogger<ClickService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task RecordClick(string shortCode)
    {
        _logger.LogInformation("Into record click function");

        _logger.LogInformation("Getting url from db");
        var url = await _repository.GetUrlByShortUrlAsync(shortCode);

        if (url == null) throw new Exception("Url not found");


        _logger.LogInformation("Updating count...");
        url.Count++;

        await _repository.SaveChangesAsync();
    }
}
