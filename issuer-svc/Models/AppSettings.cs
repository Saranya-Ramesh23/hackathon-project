namespace IssuerSvc.Models;

public class AppSettings
{
    public KestrelSettings Kestrel { get; set; }
    public LoggingSettings Logging { get; set; }
}

public class KestrelSettings
{
    public EndpointsSettings Endpoints { get; set; }
}

public class EndpointsSettings
{
    public HttpsSettings Https { get; set; }
}

public class HttpsSettings
{
    public string Url { get; set; }
    public string RegistryUrl { get; set; }
    public CertificateSettings Certificate { get; set; }
    public CertificateSettings CACertificate { get; set; }
}

public class CertificateSettings
{
    public string Path { get; set; }
    public string Password { get; set; }
}

public class LoggingSettings
{
    public LogLevelSettings LogLevel { get; set; }
}

public class LogLevelSettings
{
    public string Default { get; set; }
    public string Microsoft { get; set; }
}