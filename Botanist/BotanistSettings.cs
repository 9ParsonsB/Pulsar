using Observatory.Framework;

namespace Botanist;

class BotanistSettings
{
    [SettingDisplayName("Enable Sampler Status Overlay")]
    public bool OverlayEnabled { get; set; }

    [SettingDisplayName("Status Overlay is sticky until sampling is complete")]
    public bool OverlayIsSticky { get; set; }
}