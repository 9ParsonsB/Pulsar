namespace Pulsar.Features.Overlay.Native;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

public sealed class NativeOverlayApplication : Application
{
    private static IOverlayStateService? overlayStateService;
    private static OverlayConfiguration? overlayConfiguration;
    private static ILogger? overlayLogger;

    public static void Configure(
        IOverlayStateService stateService,
        OverlayConfiguration configuration,
        ILogger logger)
    {
        overlayStateService = stateService;
        overlayConfiguration = configuration;
        overlayLogger = logger;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (overlayStateService is null || overlayConfiguration is null)
                throw new InvalidOperationException("Native overlay was started before it was configured.");

            desktop.MainWindow = new NativeOverlayWindow(
                overlayStateService,
                overlayConfiguration,
                overlayLogger);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
