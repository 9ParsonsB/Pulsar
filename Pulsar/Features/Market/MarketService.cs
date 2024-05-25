namespace Pulsar.Features.Market;

using Observatory.Framework.Files;

public interface IMarketService : IJournalHandler<MarketFile>;

public class MarketService(IOptions<PulsarConfiguration> options, IEventHubContext hub, ILogger<MarketService> logger) : IMarketService
{
    public async Task<MarketFile> Get()
    {
        var filePath = Path.Combine(options.Value.JournalDirectory, FileName);

        if (!FileHelper.ValidateFile(filePath))
        {
            return new MarketFile();
        }

        await using var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var market = await JsonSerializer.DeserializeAsync<MarketFile>(file);
        if (market != null) return market;

        logger.LogWarning("Failed to deserialize market file {File}", filePath);
        return new MarketFile();
    }

    public async Task HandleFile(string path, CancellationToken token = new ())
    {
        if (!FileHelper.ValidateFile(path))
        {
            return;
        }

        var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var market = await JsonSerializer.DeserializeAsync<MarketFile>(file, cancellationToken: token);

        if (market == null)
        {
            logger.LogWarning("Failed to deserialize market File {FilePath}", file);
            return;
        }

        await hub.Clients.All.MarketUpdated(market);
    }

    public string FileName => FileHandlerService.MarketFileName;
}