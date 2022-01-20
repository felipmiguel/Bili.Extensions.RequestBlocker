using Bili.Extensions.RequestBlocker;

var builder = WebApplication.CreateBuilder();

var services = builder.Services;

var configuration = builder.Configuration;

// Add services to the container.

services.AddClientCertificateFilter(configuration, "CertificateFilter");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.RequireCertificate;
        httpsOptions.AllowAnyClientCertificate();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseBlocker();

app.Run(async context =>
{
    context.Response.StatusCode = 200;
    await context.Response.StartAsync();
    await context.Response.WriteAsync("Successfuly executed");
    
});

app.Run();

public partial class Program { }