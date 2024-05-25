namespace Pulsar.Features;

using System.Collections.Concurrent;
using Microsoft.Extensions.FileProviders;

public class FileWatcherService(IOptions<PulsarConfiguration> options, IFileHandlerService fileHandlerService, ILogger<FileWatcherService> logger)
    : IHostedService, IDisposable
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
        HandleFileChanged(cancellationToken);


        return Task.CompletedTask;
    }

    ConcurrentDictionary<string, DateTimeOffset> FileDates = new();

    private void HandleFileChanged(CancellationToken token = new())
    {
        Watch(token);
        var tasks = new List<Task>();
        try
        {
            foreach (var file in watcher.GetDirectoryContents(""))
            {
                logger.LogDebug("Checking File: {File}", file.PhysicalPath);
                if (file.IsDirectory || (!file.Name.EndsWith(".json") &&
                                         !(file.Name.StartsWith(FileHandlerService.JournalLogFileNameStart) &&
                                           file.Name.EndsWith(FileHandlerService.JournalLogFileNameEnd))))
                {
                    continue;
                }

                logger.LogDebug("Has File Updated?: {File}, {LastModified}", file.PhysicalPath, file.LastModified);

                FileDates.AddOrUpdate(file.PhysicalPath, _ =>
                {
                    logger.LogDebug("New File: {File}", file.PhysicalPath);
                    tasks.Add(Task.Run(() => fileHandlerService.HandleFile(file.PhysicalPath, token), token));
                    return file.LastModified;
                }, (_, existing) =>
                {
                    logger.LogDebug("Existing File: {File}", file.PhysicalPath);
                    if (existing != file.LastModified)
                    {
                        logger.LogDebug("File Updated: {File}", file.PhysicalPath);
                        tasks.Add(Task.Run(() => fileHandlerService.HandleFile(file.PhysicalPath, token), token));
                    }

                    return file.LastModified;
                });
            }

            Task.WaitAll(tasks.ToArray(), token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling file change");
        }
    }

    private void Watch(CancellationToken token)
    {
        void Handle(object? _)
        {
            HandleFileChanged(token);
        }

        try
        {
            watcher.Watch("*.*").RegisterChangeCallback(Handle, null);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error watching directory {Directory}", watcher.Root);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        watcher.Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        watcher.Dispose();
    }
}