using System.IO.Compression;
using System.Reflection;
using System.Runtime.Loader;
using Observatory.Framework;
using Pulsar.Utils;

namespace Pulsar.PluginManagement;

public class PluginManager
{
    public static PluginManager GetInstance
    {
        get
        {
                return _instance.Value;
            }
    }

    private static readonly Lazy<PluginManager> _instance = new Lazy<PluginManager>(NewPluginManager);

    private static PluginManager NewPluginManager()
    {
            return new PluginManager();
        }


    public readonly List<(string error, string? detail)> errorList;
    public readonly List<(IObservatoryWorker plugin, PluginStatus signed)> workerPlugins;
    public readonly List<(IObservatoryNotifier plugin, PluginStatus signed)> notifyPlugins;
    private readonly PluginCore core;
    private readonly PluginEventHandler pluginHandler;
        
    private PluginManager()
    {
            errorList = LoadPlugins(out workerPlugins, out notifyPlugins);

            pluginHandler = new PluginEventHandler(workerPlugins.Select(p => p.plugin), notifyPlugins.Select(p => p.plugin));
            var logMonitor = LogMonitor.GetInstance;

            logMonitor.JournalEntry += pluginHandler.OnJournalEvent;
            logMonitor.StatusUpdate += pluginHandler.OnStatusUpdate;
            logMonitor.LogMonitorStateChanged += pluginHandler.OnLogMonitorStateChanged;

            core = new PluginCore();

            List<IObservatoryPlugin> errorPlugins = new();
            
            foreach (var plugin in workerPlugins.Select(p => p.plugin))
            {
                try
                {
                    LoadSettings(plugin);
                    plugin.Load(core);
                }
                catch (PluginException ex)
                {
                    errorList.Add((FormatErrorMessage(ex), ex.StackTrace));
                    errorPlugins.Add(plugin);
                }
            }

            workerPlugins.RemoveAll(w => errorPlugins.Contains(w.plugin));
            errorPlugins.Clear();

            foreach (var plugin in notifyPlugins.Select(p => p.plugin))
            {
                // Notifiers which are also workers need not be loaded again (they are the same instance).
                if (!plugin.GetType().IsAssignableTo(typeof(IObservatoryWorker)))
                {
                    try
                    {
                        LoadSettings(plugin);
                        plugin.Load(core);
                    }
                    catch (PluginException ex)
                    {
                        errorList.Add((FormatErrorMessage(ex), ex.StackTrace));
                        errorPlugins.Add(plugin);
                    }
                    catch (Exception ex)
                    {
                        errorList.Add(($"{plugin.ShortName}: {ex.Message}", ex.StackTrace));
                        errorPlugins.Add(plugin);
                    }
                }
            }

            notifyPlugins.RemoveAll(n => errorPlugins.Contains(n.plugin));

            core.Notification += pluginHandler.OnNotificationEvent;
            core.PluginMessage += pluginHandler.OnPluginMessageEvent;

            if (errorList.Any())
                ErrorReporter.ShowErrorPopup("Plugin Load Error" + (errorList.Count > 1 ? "s" : string.Empty), errorList);
        }

    private static string FormatErrorMessage(PluginException ex)
    {
            return $"{ex.PluginName}: {ex.UserMessage}";
        }

    private void LoadSettings(IObservatoryPlugin plugin)
    {
            throw new NotImplementedException();
        }

    public static Dictionary<PropertyInfo, string> GetSettingDisplayNames(object settings)
    {
            var settingNames = new Dictionary<PropertyInfo, string>();

            if (settings != null)
            {
                var properties = settings.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var attrib = property.GetCustomAttribute<SettingDisplayName>();
                    if (attrib == null)
                    {
                        settingNames.Add(property, property.Name);
                    }
                    else
                    {
                        settingNames.Add(property, attrib.DisplayName);
                    }
                }
            }
            return settingNames;
        }

    public void SaveSettings(IObservatoryPlugin plugin, object settings)
    {
            throw new NotImplementedException();
        }

    public void SetPluginEnabled(IObservatoryPlugin plugin, bool enabled)
    {
            pluginHandler.SetPluginEnabled(plugin, enabled);
        }

    private static List<(string, string?)> LoadPlugins(out List<(IObservatoryWorker plugin, PluginStatus signed)> observatoryWorkers, out List<(IObservatoryNotifier plugin, PluginStatus signed)> observatoryNotifiers)
    {
            observatoryWorkers = new();
            observatoryNotifiers = new();
            var errorList = new List<(string, string?)>();

            var pluginPath = $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}plugins";
            
            if (Directory.Exists(pluginPath))
            {
                ExtractPlugins(pluginPath);

                var pluginLibraries = Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}plugins", "*.dll");
                foreach (var dll in pluginLibraries)
                {
                    try
                    {
                        var pluginStatus = PluginStatus.SigCheckDisabled;
                        var loadOkay = true;

                        if (loadOkay)
                        {
                            var error = LoadPluginAssembly(dll, observatoryWorkers, observatoryNotifiers, pluginStatus);
                            if (!string.IsNullOrWhiteSpace(error))
                            {
                                errorList.Add((error, string.Empty));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorList.Add(($"ERROR: {new FileInfo(dll).Name}, {ex.Message}", ex.StackTrace ?? string.Empty));
                        LoadPlaceholderPlugin(dll, PluginStatus.InvalidLibrary, observatoryNotifiers);
                    }
                }
            }
            return errorList;
        }
        
    private static void ExtractPlugins(string pluginFolder)
    {
            var files = Directory.GetFiles(pluginFolder, "*.zip")
                .Concat(Directory.GetFiles(pluginFolder, "*.eop")); // Elite Observatory Plugin

            foreach (var file in files)
            {
                try
                {
                    ZipFile.ExtractToDirectory(file, pluginFolder, true);
                    File.Delete(file);
                }
                catch 
                { 
                    // Just ignore files that don't extract successfully.
                }
            }
        }

    private static string LoadPluginAssembly(string dllPath, List<(IObservatoryWorker plugin, PluginStatus signed)> workers, List<(IObservatoryNotifier plugin, PluginStatus signed)> notifiers, PluginStatus pluginStatus)
    {
            var recursionGuard = string.Empty;

            AssemblyLoadContext.Default.Resolving += (context, name) => {
            
                if ((name?.Name?.EndsWith("resources")).GetValueOrDefault(false))
                {
                    return null;
                }

                // Importing Observatory.Framework in the Explorer Lua scripts causes an attempt to reload
                // the assembly, just hand it back the one we already have.
                if ((name?.Name?.StartsWith("Observatory.Framework")).GetValueOrDefault(false) || name?.Name == "ObservatoryFramework")
                {
                    return context.Assemblies.Where(a => (a.FullName?.Contains("ObservatoryFramework")).GetValueOrDefault(false)).First();
                }

                var foundDlls = Directory.GetFileSystemEntries(new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}plugins{Path.DirectorySeparatorChar}deps").FullName, name.Name + ".dll", SearchOption.TopDirectoryOnly);
                if (foundDlls.Any())
                {
                    return context.LoadFromAssemblyPath(foundDlls[0]);
                }

                if (name.Name != recursionGuard && name.Name != null)
                {
                    recursionGuard = name.Name;
                    return context.LoadFromAssemblyName(name);
                }

                throw new Exception("Unable to load assembly " + name.Name);
            };

            var pluginAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(new FileInfo(dllPath).FullName);
            Type[] types;
            var err = string.Empty;
            var pluginCount = 0;
            try
            {
                types = pluginAssembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.OfType<Type>().ToArray();
            }
            catch
            {
                types = Array.Empty<Type>();
            }

            IEnumerable<Type> workerTypes = types.Where(t => t.IsAssignableTo(typeof(IObservatoryWorker)));
            foreach (var worker in workerTypes)
            {
                var constructor = worker.GetConstructor(Array.Empty<Type>());
                if (constructor != null)
                {
                    var instance = constructor.Invoke(Array.Empty<object>());
                    workers.Add(((instance as IObservatoryWorker)!, pluginStatus));
                    if (instance is IObservatoryNotifier)
                    {
                        // This is also a notifier; add to the notifier list as well, so the work and notifier are
                        // the same instance and can share state.
                        notifiers.Add(((instance as IObservatoryNotifier)!, pluginStatus));
                    }
                    pluginCount++;
                }
            }

            // Filter out items which are also workers as we've already created them above.
            var notifyTypes = types.Where(t =>
                    t.IsAssignableTo(typeof(IObservatoryNotifier)) && !t.IsAssignableTo(typeof(IObservatoryWorker)));
            foreach (var notifier in notifyTypes)
            {
                var constructor = notifier.GetConstructor(Array.Empty<Type>());
                if (constructor != null)
                {
                    var instance = constructor.Invoke(Array.Empty<object>());
                    notifiers.Add(((instance as IObservatoryNotifier)!, pluginStatus));
                    pluginCount++;
                }
            }

            if (pluginCount == 0)
            {
                err += $"ERROR: Library '{dllPath}' contains no suitable interfaces.";
                LoadPlaceholderPlugin(dllPath, PluginStatus.InvalidPlugin, notifiers);
            }

            return err;
        }

    private static void LoadPlaceholderPlugin(string dllPath, PluginStatus pluginStatus, List<(IObservatoryNotifier plugin, PluginStatus signed)> notifiers)
    {
            PlaceholderPlugin placeholder = new(new FileInfo(dllPath).Name);
            notifiers.Add((placeholder, pluginStatus));
        }

    /// <summary>
    /// Possible plugin load results and signature statuses.
    /// </summary>
    public enum PluginStatus
    {
        /// <summary>
        /// Plugin valid and signed with matching certificate.
        /// </summary>
        Signed,
        /// <summary>
        /// Plugin valid but not signed with any certificate.
        /// </summary>
        Unsigned,
        /// <summary>
        /// Plugin valid but not signed with valid certificate.
        /// </summary>
        InvalidSignature,
        /// <summary>
        /// Plugin invalid and cannot be loaded. Possible version mismatch.
        /// </summary>
        InvalidPlugin,
        /// <summary>
        /// Plugin not a CLR library.
        /// </summary>
        InvalidLibrary,
        /// <summary>
        /// Plugin valid but executing assembly has no certificate to match against.
        /// </summary>
        NoCert,
        /// <summary>
        /// Plugin signature checks disabled.
        /// </summary>
        SigCheckDisabled
    }
}