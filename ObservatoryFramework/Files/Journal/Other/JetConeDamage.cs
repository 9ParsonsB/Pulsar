namespace Observatory.Framework.Files.Journal.Other;

/// <summary>
///     Written when passing through the jet code from a white dwarf or neutron star has caused damage to a ship module.
/// </summary>
public class JetConeDamage : JournalBase
{
    public string Module { get; init; }
}