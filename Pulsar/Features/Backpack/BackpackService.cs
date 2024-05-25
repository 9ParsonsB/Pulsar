namespace Pulsar.Features.Backpack;

using Observatory.Framework.Files;

public interface IBackpackService : IJournalHandler<BackpackFile>;

public class BackpackService(IOptions<PulsarConfiguration> options, IEventHubContext hub, ILogger<BackpackService> logger) : IBackpackService
{
    public async Task<BackpackFile> Get()
    {
        var filePath = Path.Combine(options.Value.JournalDirectory, FileName);

        if (!FileHelper.ValidateFile(filePath))
        {
            return new BackpackFile();
        }

        await using var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var backpack = await JsonSerializer.DeserializeAsync<BackpackFile>(file);
        if (backpack != null) return backpack;

        logger.LogWarning("Failed to deserialize backpack file {File}", filePath);
        return new BackpackFile();
    }

    public async Task HandleFile(string path, CancellationToken token = new ())
    {
        if (!FileHelper.ValidateFile(path))
        {
            return;
        }

        var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var backpack = await JsonSerializer.DeserializeAsync<BackpackFile>(file, cancellationToken: token);

        if (backpack == null)
        {
            logger.LogWarning("Failed to deserialize backpack {FilePath}", file);
            return;
        }

        await hub.Clients.All.BackpackUpdated(backpack);
    }

    public string FileName => FileHandlerService.BackpackFileName;
}