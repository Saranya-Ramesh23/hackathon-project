using IssuerSvc.Models;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace IssuerSvc.Services;

public class FileCaService : ICaService {
    private readonly X509Certificate2 _caCert;
    private readonly RSA _caPrivateKey;
    private readonly AppSettings _options;

    public FileCaService(IOptions<AppSettings> options) 
    {
        _options = options.Value;
        var caPath = options.Value.Kestrel.Endpoints.Https.CACertificate.Path;
        var caPwd = options.Value.Kestrel.Endpoints.Https.CACertificate.Password;
        _caCert = new X509Certificate2(caPath, caPwd, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
        _caPrivateKey = _caCert.GetRSAPrivateKey() ?? throw new InvalidOperationException("CA private key required");
    }

    public X509Certificate2 GetCaCertificate() => _caCert;

    public (byte[] certBytes, string thumbprint) IssueDeviceCertificate(string deviceId) 
    {
        var name = new X500DistinguishedName($"CN={deviceId}");

        using var key = RSA.Create(2048);

        var req = new CertificateRequest(name, key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        req.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
        req.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(req.PublicKey, false));
        req.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, false));

        var notBefore = DateTimeOffset.UtcNow.AddMinutes(-5);
        var notAfter = notBefore.AddYears(1);

        byte[] serial = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(serial);

        using var issuedCert = req.Create(_caCert, notBefore, notAfter, serial);

        using var certWithPrivate = issuedCert.CopyWithPrivateKey(key);

        var pfxBytes = certWithPrivate.Export(X509ContentType.Pfx, _options.Kestrel.Endpoints.Https.CACertificate.Password);
        var thumbprint = certWithPrivate.Thumbprint ?? "";

        return (pfxBytes, thumbprint);
    }
}