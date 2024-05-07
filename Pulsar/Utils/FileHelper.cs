namespace Pulsar.Utils;

public static class FileHelper
{
    public static bool ValidateFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return false;
        }

        var fileInfo = new FileInfo(filePath);

        if (fileInfo.Length == 0)
        {
            return false;
        }

        return true;
    }
}