using System.Collections.Immutable;

namespace Observatory.Framework.Files.Journal.Other;

public class WingJoin : JournalBase
{
    public ImmutableList<string> Others { get; init; }
}