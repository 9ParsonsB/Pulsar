namespace Pulsar.Features.Overlay;

public class OverlayConfiguration
{
    public bool Enabled { get; set; }

    public bool NativeOverlayEnabled { get; set; } = true;

    public bool ForceX11OnWayland { get; set; }

    public bool EnableKdeWaylandIntegration { get; set; } = true;

    public int RefreshMilliseconds { get; set; } = 250;

    public int Padding { get; set; } = 18;

    public int PanelWidth { get; set; } = 420;

    public int PanelPadding { get; set; } = 14;

    public int PanelGap { get; set; } = 10;

    public double Scale { get; set; } = 1.0;

    public int X { get; set; } = 24;

    public int Y { get; set; } = 24;

    public int Width { get; set; } = 420;

    public int Height { get; set; } = 360;
}
