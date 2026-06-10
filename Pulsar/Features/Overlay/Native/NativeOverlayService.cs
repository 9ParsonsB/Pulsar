namespace Pulsar.Features.Overlay.Native;

using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Microsoft.Extensions.Options;

public sealed class NativeOverlayService(
    IOptions<OverlayConfiguration> options,
    IOverlayStateService overlayStateService,
    ILogger<NativeOverlayService> logger,
    IHostApplicationLifetime applicationLifetime) : IHostedService
{
    private Thread? uiThread;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        applicationLifetime.ApplicationStarted.Register(StartOverlayThread);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.Shutdown();
        });

        if (uiThread is not null && uiThread.IsAlive)
            uiThread.Join(TimeSpan.FromSeconds(3));
    }

    private void StartOverlayThread()
    {
        var config = options.Value;
        if (!config.Enabled || !config.NativeOverlayEnabled)
            return;

        uiThread = new Thread(() => RunOverlay(config))
        {
            IsBackground = true,
            Name = "PulsarNativeOverlay"
        };

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            uiThread.SetApartmentState(ApartmentState.STA);

        uiThread.Start();
    }

    private void RunOverlay(OverlayConfiguration config)
    {
        try
        {
            ApplyWaylandCompatibility(config);
            NativeOverlayApplication.Configure(overlayStateService, config, logger);
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime([], ShutdownMode.OnExplicitShutdown);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Native overlay failed to start");
        }
    }

    private void ApplyWaylandCompatibility(OverlayConfiguration config)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || !config.ForceX11OnWayland)
            return;

        var session = Environment.GetEnvironmentVariable("XDG_SESSION_TYPE");
        if (!string.Equals(session, "wayland", StringComparison.OrdinalIgnoreCase))
            return;

        if (config.EnableKdeWaylandIntegration && IsKdeSession())
        {
            logger.LogInformation("KDE Wayland session detected; using native KWin integration instead of XWayland fallback.");
            return;
        }

        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("AVALONIA_PLATFORM")))
            return;

        Environment.SetEnvironmentVariable("AVALONIA_PLATFORM", "x11");
        logger.LogInformation("Wayland session detected; forcing Avalonia X11 backend so the native overlay can use XWayland input-region click-through.");
    }

    private static bool IsKdeSession()
    {
        return IsKdeValue(Environment.GetEnvironmentVariable("XDG_CURRENT_DESKTOP"))
               || IsKdeValue(Environment.GetEnvironmentVariable("XDG_SESSION_DESKTOP"))
               || string.Equals(Environment.GetEnvironmentVariable("KDE_FULL_SESSION"), "true", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsKdeValue(string? value)
    {
        return !string.IsNullOrWhiteSpace(value)
               && value.Split(':', ';')
                   .Any(part => string.Equals(part.Trim(), "KDE", StringComparison.OrdinalIgnoreCase));
    }

    private static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<NativeOverlayApplication>()
            .UsePlatformDetect()
            .LogToTrace();
    }
}
