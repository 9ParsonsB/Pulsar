namespace Pulsar.Features.ModulesInfo;

[ApiController]
[Route("api/modulesinfo")]
public class ModulesInfoController(IModulesInfoService modulesInfoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await modulesInfoService.Get());
    }
}