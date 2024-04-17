namespace Pulsar.Features.Status;

[ApiController]
[Route("api/status")]
public class StatusController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}