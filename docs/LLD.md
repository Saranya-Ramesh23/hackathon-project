# Low Level Design (LLD)

## Registry Service
- Language: C# (.NET 7)
- Key modules: DevicesController, InMemoryDeviceStore (replaceable with DB)
- Security: Certificate authentication for protected endpoints; registration may be open
- Health: /health/ready and /health/live (add using ASP.NET HealthChecks package)

## Issuer Service
- Language: C# (.NET 7)
- Key modules: IssuerController, FileCaService (development CA), InMemoryCertStore
- Security: mTLS enforcement; validate client certs against CA

## Frontend
- Angular app with DeviceService to call registry + issuer
- Does not hold private keys; browser downloads PFX for provisioning
