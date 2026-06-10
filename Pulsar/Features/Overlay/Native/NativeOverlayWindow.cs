namespace Pulsar.Features.Overlay.Native;

using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;

public sealed class NativeOverlayWindow : Window
{
    private readonly NativeOverlaySurface surface;
    private readonly DispatcherTimer refreshTimer;
    private readonly OverlayConfiguration configuration;
    private readonly ILogger? logger;

    public NativeOverlayWindow(
        IOverlayStateService overlayStateService,
        OverlayConfiguration configuration,
        ILogger? logger)
    {
        this.configuration = configuration;
        this.logger = logger;

        Title = "Pulsar Overlay";
        Width = Math.Max(1, configuration.Width);
        Height = Math.Max(1, configuration.Height);
        Position = new PixelPoint(configuration.X, configuration.Y);
        CanResize = false;
        ShowInTaskbar = false;
        Topmost = true;
        SystemDecorations = SystemDecorations.None;
        Background = Brushes.Transparent;
        TransparencyLevelHint = [WindowTransparencyLevel.Transparent, WindowTransparencyLevel.Blur];
        ExtendClientAreaToDecorationsHint = true;
        ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;
        ExtendClientAreaTitleBarHeightHint = -1;

        surface = new NativeOverlaySurface(overlayStateService, configuration)
        {
            IsHitTestVisible = false,
            Focusable = false
        };
        Content = surface;

        refreshTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(Math.Max(100, configuration.RefreshMilliseconds))
        };
        refreshTimer.Tick += (_, _) => surface.InvalidateVisual();

        Opened += (_, _) =>
        {
            ApplyClickThrough();
            refreshTimer.Start();
        };
        Closed += (_, _) => refreshTimer.Stop();
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        e.Handled = false;
        base.OnPointerPressed(e);
    }

    private void ApplyClickThrough()
    {
        try
        {
            var handle = TryGetPlatformHandle();
            if (handle is null)
                return;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ApplyWindowsClickThrough(handle.Handle);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                ApplyLinuxClickThrough(handle);
            else
                logger?.LogInformation("Native overlay input passthrough is delegated to Avalonia/platform support on this OS.");
        }
        catch (Exception ex)
        {
            logger?.LogDebug(ex, "Failed to apply native overlay click-through flags");
        }
    }

    private static void ApplyWindowsClickThrough(IntPtr hwnd)
    {
        const int gwlExstyle = -20;
        const int wsExTransparent = 0x20;
        const int wsExLayered = 0x80000;
        const int wsExToolwindow = 0x80;

        var style = GetWindowLong(hwnd, gwlExstyle);
        _ = SetWindowLong(hwnd, gwlExstyle, style | wsExLayered | wsExTransparent | wsExToolwindow);
    }

    private void ApplyLinuxClickThrough(IPlatformHandle handle)
    {
        if (!IsX11Handle(handle))
        {
            if (TryApplyKdeWaylandIntegration())
                return;

            logger?.LogWarning(
                "Native overlay is running on Linux without an X11 window handle ({HandleDescriptor}). " +
                "Wayland click-through requires XWayland fallback or a compositor layer-shell implementation.",
                handle.HandleDescriptor);
            return;
        }

        ApplyX11ClickThrough(handle.Handle);
    }

    private bool TryApplyKdeWaylandIntegration()
    {
        if (!configuration.EnableKdeWaylandIntegration || !IsKdeSession())
            return false;

        var qdbus = FindExecutable("qdbus") ?? FindExecutable("qdbus6");
        if (qdbus is null)
        {
            logger?.LogWarning("KDE Wayland integration requested, but qdbus/qdbus6 was not found.");
            return false;
        }

        try
        {
            var scriptPath = WriteKWinScript();
            RunQdbus(qdbus, "org.kde.KWin", "/Scripting", "org.kde.kwin.Scripting.unloadScript", "pulsarNativeOverlay");
            var load = RunQdbus(qdbus, "org.kde.KWin", "/Scripting", "org.kde.kwin.Scripting.loadScript", scriptPath, "pulsarNativeOverlay");
            if (load.ExitCode != 0)
            {
                logger?.LogWarning("KWin overlay script load failed: {Output}", load.Output);
                return false;
            }

            var start = RunQdbus(qdbus, "org.kde.KWin", "/Scripting", "org.kde.kwin.Scripting.start");
            if (start.ExitCode != 0)
            {
                logger?.LogWarning("KWin overlay script start failed: {Output}", start.Output);
                return false;
            }

            logger?.LogInformation("Applied KDE Wayland overlay integration through KWin scripting.");
            return true;
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "KDE Wayland overlay integration failed.");
            return false;
        }
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

    private static string? FindExecutable(string executable)
    {
        var path = Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrWhiteSpace(path))
            return null;

        foreach (var directory in path.Split(Path.PathSeparator))
        {
            if (string.IsNullOrWhiteSpace(directory))
                continue;

            var candidate = Path.Combine(directory, executable);
            if (File.Exists(candidate))
                return candidate;
        }

        return null;
    }

    private static string WriteKWinScript()
    {
        var scriptPath = Path.Combine(Path.GetTempPath(), "pulsar-native-overlay-kwin.js");
        File.WriteAllText(scriptPath, """
(() => {
    const title = "Pulsar Overlay";

    function safeString(value) {
        if (value === undefined || value === null) {
            return "";
        }
        return String(value);
    }

    function setIfPresent(win, property, value) {
        try {
            if (property in win) {
                win[property] = value;
            }
        } catch (error) {
        }
    }

    function matches(win) {
        const caption = safeString(win.caption || win.captionNormal);
        const resourceName = safeString(win.resourceName).toLowerCase();
        const resourceClass = safeString(win.resourceClass).toLowerCase();
        return caption.indexOf(title) >= 0
            || resourceName.indexOf("pulsar") >= 0
            || resourceClass.indexOf("pulsar") >= 0;
    }

    function apply(win) {
        if (!win || !matches(win)) {
            return;
        }

        setIfPresent(win, "keepAbove", true);
        setIfPresent(win, "skipTaskbar", true);
        setIfPresent(win, "skipSwitcher", true);
        setIfPresent(win, "skipPager", true);
        setIfPresent(win, "noBorder", true);
        setIfPresent(win, "blockInput", true);
        setIfPresent(win, "demandsAttention", false);
    }

    function windows() {
        if (typeof workspace.windowList === "function") {
            return workspace.windowList();
        }
        return workspace.stackingOrder || [];
    }

    function applyAll() {
        const list = windows();
        for (let i = 0; i < list.length; i += 1) {
            apply(list[i]);
        }
    }

    applyAll();
    if (workspace.windowAdded) {
        workspace.windowAdded.connect(apply);
    }
    if (workspace.windowActivated) {
        workspace.windowActivated.connect(applyAll);
    }
})();
""");
        return scriptPath;
    }

    private static (int ExitCode, string Output) RunQdbus(string qdbus, params string[] arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = qdbus,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        foreach (var argument in arguments)
            startInfo.ArgumentList.Add(argument);

        using var process = Process.Start(startInfo);

        if (process is null)
            return (-1, "Failed to start qdbus");

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        if (!process.WaitForExit(3000))
        {
            process.Kill();
            return (-1, "qdbus timed out");
        }

        return (process.ExitCode, string.Join('\n', new[] { output, error }.Where(value => !string.IsNullOrWhiteSpace(value))).Trim());
    }

    private static bool IsX11Handle(IPlatformHandle handle)
    {
        var descriptor = handle.HandleDescriptor ?? string.Empty;
        return descriptor.Contains("X11", StringComparison.OrdinalIgnoreCase)
               || descriptor.Contains("XID", StringComparison.OrdinalIgnoreCase)
               || descriptor.Contains("XWindow", StringComparison.OrdinalIgnoreCase);
    }

    private void ApplyX11ClickThrough(IntPtr window)
    {
        const int shapeInput = 2;
        const int shapeSet = 0;
        const int unsorted = 0;

        var display = XOpenDisplay(null);
        if (display == IntPtr.Zero)
        {
            logger?.LogInformation("Native overlay click-through could not open an X11 display; Wayland compositors may require platform support.");
            return;
        }

        try
        {
            XShapeCombineRectangles(display, window, shapeInput, 0, 0, IntPtr.Zero, 0, shapeSet, unsorted);
            XFlush(display);
        }
        finally
        {
            XCloseDisplay(display);
        }
    }

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW", SetLastError = true)]
    private static extern nint GetWindowLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
    private static extern nint SetWindowLongPtr(IntPtr hWnd, int nIndex, nint dwNewLong);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongW", SetLastError = true)]
    private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongW", SetLastError = true)]
    private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("libX11.so.6")]
    private static extern IntPtr XOpenDisplay(string? displayName);

    [DllImport("libX11.so.6")]
    private static extern int XCloseDisplay(IntPtr display);

    [DllImport("libX11.so.6")]
    private static extern int XFlush(IntPtr display);

    [DllImport("libXext.so.6")]
    private static extern void XShapeCombineRectangles(
        IntPtr display,
        IntPtr dest,
        int destKind,
        int xOff,
        int yOff,
        IntPtr rectangles,
        int nRectangles,
        int operation,
        int ordering);

    private static nint GetWindowLong(IntPtr hWnd, int nIndex)
    {
        return IntPtr.Size == 8 ? GetWindowLongPtr(hWnd, nIndex) : GetWindowLong32(hWnd, nIndex);
    }

    private static nint SetWindowLong(IntPtr hWnd, int nIndex, nint dwNewLong)
    {
        return IntPtr.Size == 8
            ? SetWindowLongPtr(hWnd, nIndex, dwNewLong)
            : SetWindowLong32(hWnd, nIndex, (int)dwNewLong);
    }
}
