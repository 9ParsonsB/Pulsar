using System.Reflection;
using Observatory.Framework;
using Observatory.Framework.Files;
using Pulsar.Utils;
using HttpClient = System.Net.Http.HttpClient;

namespace Pulsar.PluginManagement;

public class PluginCore : IObservatoryCore
{
    public string Version => Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "0";

    public Action<Exception, string> GetPluginErrorLogger(IObservatoryPlugin plugin)
    {
        return (ex, context) =>
        {
            LoggingUtils.LogError(ex, $"from plugin {plugin.ShortName} {context}");
        };
    }

    public Status GetStatus() => LogMonitor.GetInstance.Status;
        
    public Guid SendNotification(string title, string text)
    {
        return SendNotification(new NotificationArgs { Title = title, Detail = text });
    }

    public Guid SendNotification(NotificationArgs notificationArgs)
    {
        throw new NotImplementedException();
    }

    public void CancelNotification(Guid notificationId)
    {
        throw new NotImplementedException();
    }

    public void UpdateNotification(Guid id, NotificationArgs notificationArgs)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds an item to the datagrid on UI thread to ensure visual update.
    /// </summary>
    /// <param name="worker"></param>
    /// <param name="item"></param>
    public void AddGridItem(IObservatoryWorker worker, object item)
    {
        worker.PluginUI.DataGrid.Add(item);
    }

    public void AddGridItems(IObservatoryWorker worker, IEnumerable<Dictionary<string,string>> items)
    {
            
    }

    public void SetGridItems(IObservatoryWorker worker, IEnumerable<Dictionary<string,string>> items)
    {
            
    }
        

    public HttpClient HttpClient
    {
        get => Utils.HttpClient.Client;
    }

    public LogMonitorState CurrentLogMonitorState
    {
        get => LogMonitor.GetInstance.CurrentState;
    }

    public bool IsLogMonitorBatchReading
    {
        get => LogMonitorStateChangedEventArgs.IsBatchRead(LogMonitor.GetInstance.CurrentState);
    }

    public event EventHandler<NotificationArgs> Notification;

    internal event EventHandler<PluginMessageArgs> PluginMessage;

    public string PluginStorageFolder
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public void SendPluginMessage(IObservatoryPlugin plugin, object message)
    {
        PluginMessage?.Invoke(this, new PluginMessageArgs(plugin.Name, plugin.Version, message));
    }
}