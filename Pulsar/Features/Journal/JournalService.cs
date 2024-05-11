namespace Pulsar.Features.Journal;

using System.Text.RegularExpressions;
using Observatory.Framework.Files.Journal;

public interface IJournalService : IJournalHandler<List<JournalBase>>;

public class JournalService
(
    ILogger<JournalService> logger,
    IOptions<PulsarConfiguration> options,
    IEventHubContext hub,
    PulsarContext context
) : IJournalService
{
    public string FileName => FileHandlerService.JournalLogFileName;
    
    public Task HandleFile(string filePath) => HandleFile(filePath, CancellationToken.None);
    public async Task HandleFile(string filePath, CancellationToken token)
    {
        if (!FileHelper.ValidateFile(filePath))
        {
            return;
        }

        var file = await File.ReadAllLinesAsync(filePath, Encoding.UTF8, token);
        var journals = file.Select(line => JsonSerializer.Deserialize<JournalBase>(line)).ToList();

        
        var newJournals = new List<JournalBase>();
        var notBefore = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(6));
        foreach (var journal in journals)
        {
            if (context.Journals.Any(j => j.Timestamp == journal.Timestamp && j.Event == journal.Event))
            {
                continue;
            }

            context.Journals.Add(journal);
            
            if (journal.Timestamp > notBefore)
            {
                newJournals.Add(journal);
            }
        }

        await hub.Clients.All.JournalUpdated(newJournals);
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
