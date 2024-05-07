using Observatory.Framework.Files;

namespace Pulsar.Features.Cargo;

[ApiController]
[Route("api/[controller]")]
public class CargoController(ICargoService cargoService) : ControllerBase
{
    public async Task<ActionResult<CargoFile>> Get()
    {
        return Ok(await cargoService.Get());
    }
}