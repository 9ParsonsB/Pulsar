namespace Pulsar.Features.Overlay;

public class OverlayConfiguration
{
    public bool Enabled { get; set; }

    public bool AutoLaunchLinuxClient { get; set; }

    public string LinuxClientPath { get; set; } = "imgoverlayclient";

    public string LinuxConfigPath { get; set; } = "/tmp/pulsar-imgoverlayclient.conf";

    public string OverlayUrl { get; set; } = "http://127.0.0.1:5000/overlay";

    public string SocketPath { get; set; } = "/tmp/imgoverlay.socket";

    public int X { get; set; } = 24;

    public int Y { get; set; } = 24;

    public int Width { get; set; } = 420;

    public int Height { get; set; } = 360;
}
