namespace RegistrySvc.Models; 
public class DeviceRecord {
    public string DeviceId { get; set; } = null!;
    public string? DisplayName { get; set; }
    public System.DateTime RegisteredAt { get; set; } = System.DateTime.UtcNow;
    public string? CertificateThumbprint { get; set; }
}