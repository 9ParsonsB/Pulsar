namespace Observatory.Framework.Files.Journal;

using System.Text.Json.Nodes;

public static class JournalUtilities
{
    public const string ObsoleteMessage = "Unused in Elite Dangerous 3.7+, may appear in legacy journal data.";

    public const string UnusedMessage =
        "Documented by Frontier, but no occurances of this value ever found in real journal data.";

    public static string? GetEventType(JsonObject? line)
    {
        return line.ContainsKey("event") ? line["event"]?.ToString() : null;
    }

    public static string CleanScanEvent(string line)
    {
        return line.Replace("\"RotationPeriod\":inf,", "");
    }
}