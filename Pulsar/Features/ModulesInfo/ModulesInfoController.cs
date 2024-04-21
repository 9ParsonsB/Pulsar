namespace Pulsar.Features.ModulesInfo;

[ApiController]
[Route("api/[controller]")]
public class ModulesInfoController(IModulesInfoService modulesInfo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(modulesInfo.Get());
    }
}