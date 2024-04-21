namespace Pulsar.Features.ShipLocker;

[ApiController]
[Route("api/[controller]")]
public class ShipLockerController(IShipLockerService shipLockerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await shipLockerService.Get());
    }
}