namespace Pulsar.Features.Status;

[ApiController]
[Route("api/status")]
public class StatusController(IOptions<PulsarConfiguration> pulsarOptions, IHubContext<EventsHub, IEventsHub> hub) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // TODO: put in service
        var journalDir = pulsarOptions.Value.JournalDirectory;
        var dir = new DirectoryInfo(journalDir);
        
        if (!dir.Exists)
            return Problem("Journal directory does not exist.");
        
        var files = dir.GetFiles();
        
        var statusFile = files.FirstOrDefault(f =>
            string.Equals(f.Name, "status.json", StringComparison.InvariantCultureIgnoreCase));
        
        if (statusFile == null)
            return Problem("Status file not found.");

        await using var file = System.IO.File.Open(statusFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var status = await JsonSerializer.DeserializeAsync<Observatory.Framework.Files.Status>(file); 
        await hub.Clients.All.StatusUpdated(status);
        return Ok(status);
    }
}