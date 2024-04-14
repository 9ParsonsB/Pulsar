using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;

namespace Observatory.Framework;

/// <summary>
/// <para>Base plugin interface containing methods common to both notifiers and workers.</para>
/// <para>Note: Not intended to be implemented on its own and will not define a functional plugin. Use IObservatoryWorker, IObservatoryNotifier, or both, as appropriate.</para>
/// </summary>
public interface IObservatoryPlugin
{
    /// <summary>
    /// <para>This method will be called on startup by Observatory Core when a plugin is first loaded.</para>
    /// <para>Passes the Core interface to the plugin.</para>
    /// </summary>
    /// <param name="observatoryCore">Object implementing Observatory Core's main interface. A reference to this object should be maintained by the plugin for communication back to Core.</param>
    public void Load(IObservatoryCore observatoryCore);

    /// <summary>
    /// Full name of the plugin. Displayed in the Core settings tab's plugin list.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Short name of the plugin. Used as the tab title for the plugin UI.<br/>
    /// Can be omitted, in which case the full Name will be used.
    /// </summary>
    public string ShortName => Name;

    /// <summary>
    /// Version string displayed in the Core settings tab's plugin list.<br/>
    /// Potentially used for automated version checking. (Not yet implemented)
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Reference to plugin UI to display within its tab.
    /// </summary>
    public PluginUI PluginUI { get; }

    /// <summary>
    /// Receives data sent by other plugins.
    /// </summary>
    public void HandlePluginMessage(string sourceName, string sourceVersion, object messageArgs)
    { }
}

/// <summary>
/// <para>Interface for worker plugins which process journal data to update their UI or send notifications.</para>
/// <para>Work required on plugin startup — for example object instantiation — can be done in the constructor or Load() method.<br/>
/// Be aware that saved settings will not be available until Load() is called.</para>
/// </summary>
public interface IObservatoryWorker : IObservatoryPlugin
{
    /// <summary>
    /// Method called when new journal data is processed. Most work done by worker plugins will occur here.
    /// </summary>
    /// <typeparam name="TJournal">Specific type of journal entry being received.</typeparam>
    /// <param name="journal"><para>Elite Dangerous journal event, deserialized into a .NET object.</para>
    /// <para>Unhandled json values within a journal entry type will be contained in member property:<br/>Dictionary&lt;string, object&gt; AdditionalProperties.</para>
    /// <para>Unhandled journal event types will be type JournalBase with all values contained in AdditionalProperties.</para></param>
    public void JournalEvent<TJournal>(TJournal journal) where TJournal : JournalBase;

    /// <summary>
    /// Method called when status.json content is updated.<br/>
    /// Can be omitted for plugins which do not use this data.
    /// </summary>
    /// <param name="status">Player status.json content, deserialized into a .NET object.</param>
    public void StatusChange(Status status)
    { }

    /// <summary>
    /// Called when the LogMonitor changes state. Useful for suppressing output in certain situations
    /// such as batch reads (ie. "Read all") or responding to other state transitions.
    /// </summary>
    public void LogMonitorStateChanged(LogMonitorStateChangedEventArgs eventArgs)
    { }
}

/// <summary>
/// <para>Interface for notifier plugins which receive notification events from other plugins for any purpose.</para>
/// <para>Work required on plugin startup — for example object instantiation — can be done in the constructor or Load() method.<br/>
/// Be aware that saved settings will not be available until Load() is called.</para>
/// </summary>
public interface IObservatoryNotifier : IObservatoryPlugin
{
    /// <summary>
    /// Method called when other plugins send notification events to Observatory Core.
    /// </summary>
    /// <param name="notificationEventArgs">Details of the notification as sent from the originating worker plugin.</param>
    public void OnNotificationEvent(NotificationArgs notificationEventArgs);
}

