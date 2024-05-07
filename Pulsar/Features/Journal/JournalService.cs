namespace Pulsar.Features.Journal;

using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;

public interface IJournalService : IJournalHandler<List<JournalBase>>;

public class JournalService
(
    ILogger<JournalService> logger,
    IOptions<PulsarConfiguration> options,
    IEventHubContext hub
) : IJournalService
{
    public string FileName => "Journal.2024-03-16T152419.01.log"; // FileHandlerService.JournalLogFileName;

    public bool ValidateFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            logger.LogWarning("Journal file {JournalFile} does not exist", filePath);
            return false;
        }

        var fileInfo = new FileInfo(filePath);

        if (!string.Equals(fileInfo.Name, FileName, StringComparison.InvariantCultureIgnoreCase))
        {
            logger.LogWarning("Journal file {name} is not valid");
            return false;
        }

        if (fileInfo.Length == 0)
        {
            logger.LogWarning("Journal file {name} is empty", filePath);
            return false;
        }

        return true;
    }

    public async Task HandleFile(string filePath)
    {
        if (!ValidateFile(filePath))
        {
            return;
        }

        var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var moduleInfo = await JsonSerializer.DeserializeAsync<List<JournalBase>>(file);

        if (moduleInfo == null)
        {
            logger.LogWarning("Failed to deserialize status file {FilePath}", filePath);
            return;
        }

        // await hub.Clients.All.ModuleInfoUpdated(moduleInfo);
    }

    public async Task<List<JournalBase>> Get()
    {
        var dataFileName = Path.Combine(options.Value.JournalDirectory, FileName);

        if (!ValidateFile(dataFileName))
        {
            return [];
        }

        // Seems each entry is a new line. Not sure if this can be relied on?
        var logs = File.ReadAllLines(dataFileName);

        var journals = new List<JournalBase>();
        foreach (var log in logs)
        {
            // var info = JournalReader.ObservatoryDeserializer<JournalBase>(log);
            var info = JsonSerializer.Deserialize<JournalBase>(log);
            if (info != null)
            {
                journals.Add(info);
            }
        }

        if (journals.Count > 0) return journals;

        logger.LogWarning("Failed to deserialize module info file {file}", dataFileName);
        return [];
    }
}
