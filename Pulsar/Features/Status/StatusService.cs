namespace Pulsar.Features.Status;

using Observatory.Framework.Files;

public interface IStatusService : IJournalHandler<Status>;
public class StatusService(ILogger<StatusService> logger, IOptions<PulsarConfiguration> options, IEventHubContext hub) : IStatusService
{
    public string FileName => FileHandlerService.StatusFileName;
    
    public async Task HandleFile(string filePath)
    {
        if (!ValidateFile(filePath))
        {
            return;
        }
        
        var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var status = await JsonSerializer.DeserializeAsync<Status>(file); 
        
        if (status == null)
        {
            logger.LogWarning("Failed to deserialize status file {FilePath}", filePath);
            return;
        }
        
        await hub.Clients.All.StatusUpdated(status);
    }

    public bool ValidateFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            logger.LogWarning("Status file {StatusFile} does not exist", filePath);
            return false;
        }
        
        var fileInfo = new FileInfo(filePath);
        
        if (!string.Equals(fileInfo.Name, FileName, StringComparison.InvariantCultureIgnoreCase))
        {
            logger.LogWarning("File {StatusFile} is not a status file", filePath);
            return false;
        }

        if (fileInfo.Length == 0)
        {
            logger.LogWarning("Status file {StatusFile} is empty", filePath);
            return false;
        }

        return true;
    }

    public async Task<Status> Get()
    {
        var statusFile = Path.Combine(options.Value.JournalDirectory, FileName);
        
        if (!ValidateFile(statusFile))
        {
            return new Status();
        }

        await using var file = File.Open(statusFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var status = await JsonSerializer.DeserializeAsync<Status>(file);
        if (status != null) return status;
        
        logger.LogWarning("Failed to deserialize status file {StatusFile}", statusFile);
        return new Status();
    }

}