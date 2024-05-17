namespace Pulsar.Features.Journal;

using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Observatory.Framework.Files.Journal;

public interface IJournalService : IJournalHandler<List<JournalBase>>;

public class JournalService(
    ILogger<JournalService> logger,
    IOptions<PulsarConfiguration> options,
    IEventHubContext hub,
    PulsarContext context,
    IServiceProvider serviceProvider
) : IJournalService
{
    public string FileName => FileHandlerService.JournalLogFileName;

    static ConcurrentBag<JournalBase> _journals = new();
    
    static DateTimeOffset notBefore = DateTimeOffset.UtcNow.AddHours(-1);
    
    readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowOutOfOrderMetadataProperties = true,
        // Converters = { ActivatorUtilities.CreateInstance<JournalJsonConverter>(serviceProvider) }
    };

    public async Task HandleFile(string filePath)
    {
        if (!FileHelper.ValidateFile(filePath))
        {
            return;
        }

        var file = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
        var newJournals = new List<JournalBase>();
        await Parallel.ForEachAsync(file, (line, _) =>
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return ValueTask.CompletedTask;
            }

            var journal = JsonSerializer.Deserialize<JournalBase>(line, options);
            if (journal == null)
            {
                return ValueTask.CompletedTask;
            }

            if (_journals.Any(j => j.Timestamp == journal.Timestamp && j.GetType() == journal.GetType()))
            {
                return ValueTask.CompletedTask;
            }
            
            _journals.Add(journal);
            
            if (journal.Timestamp < notBefore)
            {
                return ValueTask.CompletedTask;
            }

            newJournals.Add(journal);
            return ValueTask.CompletedTask;
        });
        

        if (newJournals.Any())
        {
            await hub.Clients.All.JournalUpdated(newJournals);
        }
    }

    public async Task<List<JournalBase>> Get()
    {
        return [];
    }
}