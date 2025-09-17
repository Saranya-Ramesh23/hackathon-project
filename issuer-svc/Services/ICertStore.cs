using System.Collections.Concurrent;

namespace IssuerSvc.Services; 
public interface ICertStore 
{
    Task StoreCertificateAsync(string deviceId, byte[] pfxBytes, string thumbprint);
    Task<byte[]?> GetCertificateAsync(string deviceId);
}