namespace Pulsar.Features.Journal;

using System.Collections.Concurrent;

public interface IJournalStore
{
    void EnqueueFile(string filePath);
    bool TryDequeue(out string filePath);
}

public class JournalStore : IJournalStore
{
    private readonly ConcurrentQueue<string> JournalFileQueue = new();  
    
    public void EnqueueFile(string filePath)
    {
        JournalFileQueue.Enqueue(filePath);
    }
    
    public bool TryDequeue(out string filePath)
    {
        return JournalFileQueue.TryDequeue(out filePath);
    }
}