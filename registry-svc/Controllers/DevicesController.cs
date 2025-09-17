using Microsoft.AspNetCore.Mvc;
using RegistrySvc.Models;
using RegistrySvc.Services;

namespace RegistrySvc.Controllers;

[ApiController]
[Route("registry/devices")]
public class DevicesController : ControllerBase {
    private readonly IDeviceStore _store;
    private readonly ILogger<DevicesController> _log;

    public DevicesController(IDeviceStore store, ILogger<DevicesController> log) {
        _store = store;
        _log = log;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] DeviceRecord model) 
    {
        if (string.IsNullOrWhiteSpace(model.DeviceId)) 
            return BadRequest("deviceId required");

        var rec = new DeviceRecord {
            DeviceId = model.DeviceId,
            DisplayName = model.DisplayName
        };

        var added = await _store.AddDeviceAsync(rec);
        if (!added) return Conflict("device already exists");

        _log.LogInformation("Device registered {deviceId} correlation={correlation}", rec.DeviceId, HttpContext.TraceIdentifier);
        
        return CreatedAtAction(nameof(Get), new { deviceId = rec.DeviceId }, rec);
    }

    [HttpGet("{deviceId}")]
    public async Task<IActionResult> Get(string deviceId) 
    {
        var rec = await _store.GetDeviceAsync(deviceId);
        if (rec == null) 
            return NotFound();
       
        return Ok(rec);
    }
}
