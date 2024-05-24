namespace Pulsar.Features;

using System.Collections.Concurrent;
using Microsoft.Extensions.FileProviders;

public class FileWatcherService(IOptions<PulsarConfiguration> options, IFileHandlerService fileHandlerService)
    : IHostedService
{
    private PhysicalFileProvider watcher = null!;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Directory.Exists(options.Value.JournalDirectory))
        {
            throw new Exception($"Directory {options.Value.JournalDirectory} does not exist.");
        }

        watcher = new PhysicalFileProvider(options.Value.JournalDirectory);
        Watch(cancellationToken);

        // read the journal directory to get the initial files
#if DEBUG
        Task.Run(() =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            HandleFileChanged(cancellationToken);
        }, cancellationToken);
#else
        HandleFileChanged(cancellationToken);
#endif


        return Task.CompletedTask;
    }

    ConcurrentDictionary<string, DateTimeOffset> FileDates = new();

    private void HandleFileChanged(CancellationToken token = new())
    {
        var tasks = new List<Task>();
        foreach (var file in watcher.GetDirectoryContents(""))
        {
            if (file.IsDirectory || !file.Name.EndsWith(".json") &&
                !(file.Name.StartsWith(FileHandlerService.JournalLogFileNameStart) &&
                  file.Name.EndsWith(FileHandlerService.JournalLogFileNameEnd)))
            {
                return;
            }


            FileDates.AddOrUpdate(file.PhysicalPath, _ =>
            {
                tasks.Add(Task.Run(() => fileHandlerService.HandleFile(file.PhysicalPath, token), token));
                return file.LastModified;
            }, (_, existing) =>
            {
                if (existing != file.LastModified)
                {
                    tasks.Add(Task.Run(() => fileHandlerService.HandleFile(file.PhysicalPath, token), token));
                }

                return file.LastModified;
            });
        }

        Watch(token);
        
        Task.WaitAll(tasks.ToArray(), token);
    }

    private void Watch(CancellationToken token)
    {
        void Handle(object? _)
        {
            HandleFileChanged(token);
        }

        watcher.Watch("*.*").RegisterChangeCallback(Handle, null);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        watcher.Dispose();
        return Task.CompletedTask;
    }
}