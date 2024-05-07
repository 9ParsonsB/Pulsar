namespace Pulsar.Features.Journal;

using System.Text.RegularExpressions;
using Observatory.Framework.Files.Journal;

public interface IJournalService : IJournalHandler<List<JournalBase>>;

public class JournalService
(
    ILogger<JournalService> logger,
    IOptions<PulsarConfiguration> options,
    IEventHubContext hub
) : IJournalService
{
    public string FileName => FileHandlerService.JournalLogFileName;
    
    public async Task HandleFile(string filePath)
    {
        if (!FileHelper.ValidateFile(filePath))
        {
            return;
        }

        var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var journals = await JsonSerializer.DeserializeAsync<List<JournalBase>>(file);

        if (journals == null)
        {
            logger.LogWarning("Failed to deserialize status file {FilePath}", filePath);
            return;
        }

        await hub.Clients.All.JournalUpdated(journals);
    }

    public async Task<List<JournalBase>> Get()
    {
        var folder = new DirectoryInfo(options.Value.JournalDirectory);
        var regex = new Regex(FileHandlerService.JournalLogFileNameRegEx);
        
        if (!folder.Exists)
        {
            logger.LogWarning("Journal directory {JournalDirectory} does not exist", folder.FullName);
            return [];
        }
        
        var dataFileName = folder.GetFiles().FirstOrDefault(f => regex.IsMatch(f.Name))?.FullName;
        
        if (!FileHelper.ValidateFile(dataFileName))
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
