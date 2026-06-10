namespace Pulsar.Features.Overlay;

using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;

public class LinuxOverlayClientService(
    IOptions<OverlayConfiguration> options,
    ILogger<LinuxOverlayClientService> logger,
    IHostApplicationLifetime applicationLifetime) : IHostedService
{
    private Process? process;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        applicationLifetime.ApplicationStarted.Register(StartClient);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (process is { HasExited: false })
                process.Kill(entireProcessTree: true);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to stop imgoverlayclient");
        }

        return Task.CompletedTask;
    }

    private void StartClient()
    {
        var config = options.Value;
        if (!config.Enabled || !config.AutoLaunchLinuxClient || !RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return;

        try
        {
            var ini = $$"""
[General]
Socket={{config.SocketPath}}
Cache=/tmp/pulsar-imgoverlay-cache

[Pulsar]
Url={{config.OverlayUrl}}
X={{config.X}}
Y={{config.Y}}
Width={{config.Width}}
Height={{config.Height}}
""";
            File.WriteAllText(config.LinuxConfigPath, ini);

            process = Process.Start(new ProcessStartInfo
            {
                FileName = config.LinuxClientPath,
                ArgumentList = { config.LinuxConfigPath },
                UseShellExecute = false
            });

            logger.LogInformation("Started Linux overlay client using {ConfigPath}", config.LinuxConfigPath);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex,
                "Failed to launch Linux overlay client. Ensure imgoverlayclient is installed and the renderer socket is available.");
        }
    }
}
