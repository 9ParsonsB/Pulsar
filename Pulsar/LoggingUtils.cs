using System.Text;

namespace Pulsar;

public static class LoggingUtils
{
    internal static void LogError(Exception ex, string context)
    {
        var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var errorMessage = new StringBuilder();
        var timestamp = DateTime.Now.ToString("G");
        errorMessage
            .AppendLine($"[{timestamp}] Error encountered in Elite Observatory {context}")
            .AppendLine(FormatExceptionMessage(ex))
            .AppendLine();
        File.AppendAllText(docPath + Path.DirectorySeparatorChar + "ObservatoryCrashLog.txt",
            errorMessage.ToString());
    }

    static string FormatExceptionMessage(Exception ex, bool inner = false)
    {
        var errorMessage = new StringBuilder();
        errorMessage
            .AppendLine($"{(inner ? "Inner e" : "E")}xception message: {ex.Message}")
            .AppendLine("Stack trace:")
            .AppendLine(ex.StackTrace);
        if (ex.InnerException != null)
            errorMessage.AppendLine(FormatExceptionMessage(ex.InnerException, true));
        return errorMessage.ToString();
    }
}