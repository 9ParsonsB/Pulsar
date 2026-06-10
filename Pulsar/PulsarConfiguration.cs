namespace Pulsar;

using System.ComponentModel.DataAnnotations;

public class PulsarConfiguration
{
    [Required]
    public string JournalDirectory { get; set; }

    public bool ProcessHistoricalJournals { get; set; } = false;
}