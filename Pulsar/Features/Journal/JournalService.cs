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

    public async Task HandleFile(string filePath)
    {
        if (!FileHelper.ValidateFile(filePath))
        {
            return;
        }

        var file = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
        var newJournals = new List<JournalBase>();
        var select = file.AsParallel().Select(line => JsonSerializer.Deserialize<JournalBase>(line,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { ActivatorUtilities.CreateInstance<JournalConverter>(serviceProvider) }
            }));
        
        foreach (var journal in select)
        {
            if (_journals.Any(j => j.Timestamp == journal.Timestamp && j.Event == journal.Event))
            {
                continue;
            }
            
            if (journal.Timestamp < notBefore)
            {
                continue;
            }

            _journals.Add(journal);
            newJournals.Add(journal);
        }

        if (newJournals.Any())
        {
            await hub.Clients.All.JournalUpdated(newJournals);
        }
    }

    public async Task<List<JournalBase>> Get()
    {
        await hub.Clients.All.JournalUpdated(_journals.ToList());
        return _journals.ToList();
    }
}