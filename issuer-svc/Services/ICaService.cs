using System.Security.Cryptography.X509Certificates;

namespace IssuerSvc.Services;

public interface ICaService {
    X509Certificate2 GetCaCertificate();
    (byte[] certBytes, string thumbprint) IssueDeviceCertificate(string deviceId);
}
