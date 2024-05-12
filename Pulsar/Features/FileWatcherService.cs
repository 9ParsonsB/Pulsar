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
        Watch();

        // read the journal directory to get the initial files
#if DEBUG
        Task.Run(() =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(10));
            HandleFileChanged();
        }, cancellationToken);
#else
        HandleFileChanged();
#endif


        return Task.CompletedTask;
    }

    ConcurrentDictionary<string, DateTimeOffset> FileDates = new();

    private void HandleFileChanged(object? sender = null)
    {
        foreach (var file in watcher.GetDirectoryContents(""))
        {
            if (file.IsDirectory || !file.Name.EndsWith(".json") &&
                !(file.Name.StartsWith(FileHandlerService.JournalLogFileNameStart) &&
                  file.Name.EndsWith(FileHandlerService.JournalLogFileNameEnd)))
            {
                continue;
            }

            FileDates.AddOrUpdate(file.PhysicalPath, _ =>
            {
                Task.Run(() => fileHandlerService.HandleFile(file.PhysicalPath));
                return file.LastModified;
            }, (_, existing) =>
            {
                if (existing != file.LastModified)
                {
                    Task.Run(() => fileHandlerService.HandleFile(file.PhysicalPath));
                }

                return file.LastModified;
            });
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