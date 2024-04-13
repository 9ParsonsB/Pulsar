using System.Collections.Immutable;

namespace Observatory.Framework.Files.Journal.Powerplay;

public class PowerplayVoucher : PowerplayJoin
{
    public ImmutableList<string> Systems { get; init; }
}