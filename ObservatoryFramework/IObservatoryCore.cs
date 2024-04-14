using Observatory.Framework.Files;

namespace Observatory.Framework;

/// <summary>
/// Interface passed by Observatory Core to plugins. Primarily used for sending notifications and UI updates back to Core.
/// </summary>
public interface IObservatoryCore
{
    /// <summary>
    /// Send a notification out to all native notifiers and any plugins implementing IObservatoryNotifier.
    /// </summary>
    /// <param name="title">Title text for notification.</param>
    /// <param name="detail">Detail/body text for notificaiton.</param>
    /// <returns>Guid associated with the notification during its lifetime. Used as an argument with CancelNotification and UpdateNotification.</returns>
    public Guid SendNotification(string title, string detail);

    /// <summary>
    /// Send a notification with arguments out to all native notifiers and any plugins implementing IObservatoryNotifier.
    /// </summary>
    /// <param name="notificationEventArgs">NotificationArgs object specifying notification content and behaviour.</param>
    /// <returns>Guid associated with the notification during its lifetime. Used as an argument with CancelNotification and UpdateNotification.</returns>
    public Guid SendNotification(NotificationArgs notificationEventArgs);

    /// <summary>
    /// Cancel or close an active notification.
    /// </summary>
    /// <param name="notificationId">Guid of notification to be cancelled.</param>
    public void CancelNotification(Guid notificationId);

    /// <summary>
    /// Update an active notification with a new set of NotificationsArgs. Timeout values are reset and begin counting again from zero if specified.
    /// </summary>
    /// <param name="notificationId">Guid of notification to be updated.</param>
    /// <param name="notificationEventArgs">NotificationArgs object specifying updated notification content and behaviour.</param>
    public void UpdateNotification(Guid notificationId, NotificationArgs notificationEventArgs);

    /// <summary>
    /// Requests current Elite Dangerous status.json content.
    /// </summary>
    /// <returns>Status object reflecting current Elite Dangerous player status.</returns>
    public Status GetStatus();

    /// <summary>
    /// Version string of Observatory Core.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Returns a delegate for logging an error for the calling plugin. A plugin can wrap this method
    /// or pass it along to its collaborators.
    /// </summary>
    /// <param name="plugin">The calling plugin</param>
    public Action<Exception, string> GetPluginErrorLogger(IObservatoryPlugin plugin);

    /// <summary>
    /// Shared application HttpClient object. Provided so that plugins can adhere to .NET recommended behaviour of a single HttpClient object per application.
    /// </summary>
    public HttpClient HttpClient { get; }

    /// <summary>
    /// Returns the current LogMonitor state.
    /// </summary>
    public LogMonitorState CurrentLogMonitorState { get; }

    /// <summary>
    /// Returns true if the current LogMonitor state represents a batch-read mode.
    /// </summary>
    public bool IsLogMonitorBatchReading { get; }

    /// <summary>
    /// Retrieves and ensures creation of a location which can be used by the plugin to store persistent data.
    /// </summary>
    public string PluginStorageFolder { get; }

    /// <summary>
    /// Sends arbitrary data to all other plugins. The full name and version of the sending plugin will be used to identify the sender to any recipients.
    /// </summary>
    /// <param name="message">Utf8 data to be sent. Must be serializable to JSON.</param>
    public void SendPluginMessage(IObservatoryPlugin plugin, ReadOnlySpan<byte> message);
}