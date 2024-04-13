using System.Reflection;
using Observatory;
using Pulsar;
using Pulsar.Utils;

SettingsManager.Load();

if (args.Length > 0 && File.Exists(args[0]))
{
    var fileInfo = new FileInfo(args[0]);
    if (fileInfo.Extension == ".eop" || fileInfo.Extension == ".zip")
        File.Copy(
            fileInfo.FullName,
            $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}plugins{Path.DirectorySeparatorChar}{fileInfo.Name}");
}

var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "0";

try
{
    //TODO: Start Application
}
catch (Exception ex)
{
    LoggingUtils.LogError(ex, version);
}