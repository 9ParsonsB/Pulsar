using Microsoft.EntityFrameworkCore;
using Observatory.Framework.Files.Journal.Startup;
using Pulsar.Context;

namespace Pulsar.Features.Status;

using Observatory.Framework.Files;

public interface IStatusService : IJournalHandler<Status>;

public class StatusService
(
    ILogger<StatusService> logger,
    IOptions<PulsarConfiguration> options,
    IEventHubContext hub,
    PulsarContext context
) : IStatusService
{
    public string FileName => FileHandlerService.StatusFileName;

    public async Task HandleFile(string filePath, CancellationToken token = new())
    {
        if (!FileHelper.ValidateFile(filePath))
        {
            return;
        }

        var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        
        if (file.Length < 2)
        {
            logger.LogWarning("File {FilePath} is empty", filePath);
            return;
        }
            
        var status = await JsonSerializer.DeserializeAsync<Status>(file, cancellationToken: token);

        if (status == null)
        {
            logger.LogWarning("Failed to deserialize status file {FilePath}", filePath);
            return;
        }

        await hub.Clients.All.StatusUpdated(status);
    }

    public async Task<Status> Get()
    {
        var statusFile = Path.Join(options.Value.JournalDirectory, FileName);

        if (!FileHelper.ValidateFile(statusFile))
        {
            return new ();
        }

        await using var file = File.Open(statusFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var status = await JsonSerializer.DeserializeAsync<Status>(file);
        if (status != null)
        {
            return status;
        }

        logger.LogWarning("Failed to deserialize status file {StatusFile}", statusFile);
        return new ();
    }
    

}