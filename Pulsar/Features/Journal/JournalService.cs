namespace Pulsar.Features.Journal;

using System.Collections.Concurrent;
using Observatory.Framework.Files.Journal;

public interface IJournalService : IJournalHandler<List<JournalBase>>
{
    public bool TryDequeue(out string filePath);
}

public class JournalService(
    ILogger<JournalService> logger
) : IJournalService
{
    public string FileName => FileHandlerService.JournalLogFileName;

    private readonly ConcurrentQueue<string> JournalFileQueue = new();
    
    public void EnqueueFile(string filePath)
    {
        JournalFileQueue.Enqueue(filePath);
    }
    
    public bool TryDequeue(out string filePath)
    {
        return JournalFileQueue.TryDequeue(out filePath);
    }

    public Task HandleFile(string filePath, CancellationToken token = new())
    {
        if (!FileHelper.ValidateFile(filePath))
        {
            return Task.CompletedTask;
        }

        EnqueueFile(filePath);
        return Task.CompletedTask;
    }
    
    public async Task<List<JournalBase>> Get()
    {
        return [];
    }
}