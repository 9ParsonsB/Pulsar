namespace Pulsar.Features.Journal;

[ApiController]
[Route("api/journal")]
public class JournalController(IJournalService journalService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await journalService.Get());
    }
}