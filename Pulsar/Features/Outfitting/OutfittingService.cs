namespace Pulsar.Features.Outfitting;

using Observatory.Framework.Files;

public interface IOutfittingService : IJournalHandler<OutfittingFile>;

public class OutfittingService(IOptions<PulsarConfiguration> options, IEventHubContext hub, ILogger<OutfittingService> logger) : IOutfittingService
{
    public async Task<OutfittingFile> Get()
    {
        var filePath = Path.Combine(options.Value.JournalDirectory, FileName);

        if (!FileHelper.ValidateFile(filePath))
        {
            return new OutfittingFile();
        }

        await using var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var outfitting = await JsonSerializer.DeserializeAsync<OutfittingFile>(file);
        if (outfitting != null) return outfitting;

        logger.LogWarning("Failed to deserialize outfitting file {File}", filePath);
        return new OutfittingFile();
    }

    public async Task HandleFile(string path, CancellationToken token = new ())
    {
        if (!FileHelper.ValidateFile(path))
        {
            return;
        }

        var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var outfitting = await JsonSerializer.DeserializeAsync<OutfittingFile>(file, cancellationToken: token);

        if (outfitting == null)
        {
            logger.LogWarning("Failed to deserialize outfitting file {FilePath}", file);
            return;
        }

        await hub.Clients.All.OutfittingUpdated(outfitting);
    }

    public string FileName => FileHandlerService.OutfittingFileName;
}