using System.Collections.Concurrent;

namespace IssuerSvc.Services;
public class CertStore : ICertStore
{
    private readonly ConcurrentDictionary<string, byte[]> _store = new();
    public Task StoreCertificateAsync(string deviceId, byte[] pfxBytes, string thumbprint)
    {
        _store[deviceId] = pfxBytes;
        return Task.CompletedTask;
    }

    public Task<byte[]?> GetCertificateAsync(string deviceId)
    {
        _store.TryGetValue(deviceId, out var b);

        return Task.FromResult<byte[]?>(b);
    }  
}