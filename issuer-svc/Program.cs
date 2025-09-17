using Microsoft.AspNetCore.Authentication.Certificate;
using IssuerSvc.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using IssuerSvc.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddSingleton<ICaService, FileCaService>();
builder.Services.AddSingleton<ICertStore, CertStore>();
builder.Services.AddHttpClient();
builder.Services.AddHealthChecks();
builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options => {
        options.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck;
        options.Events = new CertificateAuthenticationEvents {
            OnCertificateValidated = context => {
                context.Success();
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = _ => true });
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => true });

app.Run();
