using System.Text.Json.Nodes;

namespace Observatory.Framework.Files.Journal;

public static class JournalUtilities
{
    public static string? GetEventType(JsonObject? line)
    {
        return line.ContainsKey("event") ? line["event"]?.ToString() : null;
    }

    public static string CleanScanEvent(string line)
    {
        return line.Replace("\"RotationPeriod\":inf,", "");
    }

    public const string ObsoleteMessage = "Unused in Elite Dangerous 3.7+, may appear in legacy journal data.";

    public const string UnusedMessage =
        "Documented by Frontier, but no occurances of this value ever found in real journal data.";
}