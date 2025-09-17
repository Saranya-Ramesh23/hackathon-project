using IssuerSvc.Models;
using IssuerSvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IssuerSvc.Controllers; 

[ApiController]
[Route("issuer")]
public class IssuerController : ControllerBase 
{
    private readonly ICaService _ca;
    private readonly ICertStore _store;
    private readonly ILogger<IssuerController> _log;
    private readonly IOptions<AppSettings> _options;
    private readonly HttpClient _http;

    public IssuerController(ICaService ca, ICertStore store, ILogger<IssuerController> log, IHttpClientFactory factory, IOptions<AppSettings> options) 
    {
        _ca = ca;
        _store = store;
        _log = log;
        _http = factory.CreateClient();
        _options = options;
    }

    [HttpPost("enroll")]
    public async Task<IActionResult> Enroll([FromBody] EnrollRequest req) 
    {
        if (string.IsNullOrWhiteSpace(req.DeviceId)) 
            return BadRequest("deviceId required");

        var registryUrl = _options.Value.Kestrel.Endpoints.Https.RegistryUrl;
        var checkUrl = $"{registryUrl}/registry/devices/{req.DeviceId}";
        
        try 
        {
            var resp = await _http.GetAsync(checkUrl);
            if (!resp.IsSuccessStatusCode) 
            {
                _log.LogWarning("Enrollment attempt for unknown device {deviceId}", req.DeviceId);
                return BadRequest("device not registered");
            }

        } catch (Exception ex) 
        {
            _log.LogError(ex, "Error checking registry for device {deviceId}", req.DeviceId);
            return StatusCode(500, "registry check failed");
        }

        var (pfxBytes, thumbprint) = _ca.IssueDeviceCertificate(req.DeviceId);
        await _store.StoreCertificateAsync(req.DeviceId, pfxBytes, thumbprint);

        _log.LogInformation("Issued cert for {deviceId} thumb={thumbprint} correlation={cid}",
            req.DeviceId, thumbprint, HttpContext.TraceIdentifier);

        return Ok(new { message = "issued", thumbprint });
    }

    [HttpGet("certs/{deviceId}")]
    public async Task<IActionResult> Download(string deviceId) 
    {
        var cert = await _store.GetCertificateAsync(deviceId);

        if (cert == null) 
            return NotFound();

        return File(cert, "application/x-pkcs12", $"{deviceId}.pfx");
    }
}