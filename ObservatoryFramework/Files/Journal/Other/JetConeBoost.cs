namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when enough material has been collected from a solar jet code (at a white dwarf or neutron star) for a jump
///     boost.
/// </summary>
public class JetConeBoost : JournalBase
{
    public float BoostValue { get; init; }
}