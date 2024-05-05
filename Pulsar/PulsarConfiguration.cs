using System.ComponentModel.DataAnnotations;

namespace Pulsar;

public class PulsarConfiguration
{
    [Required]
    public string JournalDirectory { get; set; }
}