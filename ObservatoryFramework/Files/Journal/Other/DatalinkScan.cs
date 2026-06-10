namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when scanning a data link.
/// </summary>
public class DatalinkScan : JournalBase
{
    public string Message { get; init; }
    public string Message_Localised { get; init; }
}