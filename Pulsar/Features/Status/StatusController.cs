namespace Pulsar.Features.Status;

[ApiController]
[Route("api/[controller]")]
public class StatusController(IStatusService status) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(status.Get());
    }
}