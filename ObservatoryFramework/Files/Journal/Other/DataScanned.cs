namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when scanning some types of data links.
/// </summary>
public class DataScanned : JournalBase
{
    public string Type { get; init; }
    public string Type_Localised { get; init; }
}