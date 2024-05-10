namespace Pulsar.Features;

using System.Collections.Concurrent;
using Microsoft.Extensions.FileProviders;

public class FileWatcherService(IOptions<PulsarConfiguration> options, IFileHandlerService fileHandlerService) : IHostedService
{
    private PhysicalFileProvider watcher = null!;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Directory.Exists(options.Value.JournalDirectory))
        {
            throw new Exception($"Directory {options.Value.JournalDirectory} does not exist.");
        }
        
        watcher = new PhysicalFileProvider(options.Value.JournalDirectory);
        Watch();

        return Task.CompletedTask;
    }
    
    ConcurrentDictionary<string, DateTimeOffset> FileDates = new();

    private void HandleFileChanged(object? sender)
    {
        foreach (var file in watcher.GetDirectoryContents(""))
        {
            if (file.IsDirectory || !file.Name.EndsWith(".json") && !(file.Name.StartsWith(FileHandlerService.JournalLogFileNameStart) && file.Name.EndsWith(FileHandlerService.JournalLogFileNameEnd)))
            {
                continue;
            }
            
            var existing = FileDates.GetOrAdd(file.PhysicalPath, file.LastModified);
            
            if (existing != file.LastModified)
            {
                fileHandlerService.HandleFile(file.PhysicalPath);
            }
        }
        Watch();
    }
    
    private void Watch()
    {
        watcher.Watch("*.*").RegisterChangeCallback(HandleFileChanged, null);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        watcher.Dispose();
        return Task.CompletedTask;
    }
}