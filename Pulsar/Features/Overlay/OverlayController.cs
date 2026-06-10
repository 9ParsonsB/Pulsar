namespace Pulsar.Features.Overlay;

using Microsoft.AspNetCore.Mvc;

[ApiController]
public class OverlayController(IOverlayStateService overlayStateService) : ControllerBase
{
    [HttpGet("/api/overlay/state")]
    public ActionResult<OverlaySnapshot> GetState()
    {
        return Ok(overlayStateService.GetSnapshot());
    }
}
