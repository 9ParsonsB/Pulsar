namespace Pulsar.Features;

public class FileWatcherService(IOptions<PulsarConfiguration> options, IFileHandlerService fileHandlerService) : IHostedService
{
    private FileSystemWatcher watcher = null!;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        watcher = new FileSystemWatcher(options.Value.JournalDirectory)
        {
            EnableRaisingEvents = true,
            IncludeSubdirectories = true
        };
        
        watcher.BeginInit();
        
        watcher.Created += HandleFileChanged;
        watcher.Changed += HandleFileChanged;
        watcher.Renamed += HandleFileChanged; // ?
        
        watcher.IncludeSubdirectories = false;
        watcher.EnableRaisingEvents = true;
        watcher.NotifyFilter = NotifyFilters.LastWrite & NotifyFilters.Size & NotifyFilters.FileName;
        
        watcher.EndInit();
        
        return Task.CompletedTask;
    }

    private void HandleFileChanged(object sender, FileSystemEventArgs e)
    {
        fileHandlerService.HandleFile(e.FullPath);   
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        watcher.Dispose();
        return Task.CompletedTask;
    }
}