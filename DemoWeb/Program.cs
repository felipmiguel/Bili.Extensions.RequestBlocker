using Bili.Extensions.RequestBlocker;

//var builder = WebApplication.CreateBuilder(args);

//var services = builder.Services;

//var configuration = builder.Configuration;

//// Add services to the container.
//services.AddRazorPages();
//services.AddClientCertificateFilter(configuration, "CertificateFilter");

//builder.WebHost.ConfigureKestrel(options => options.ConfigureHttpsDefaults(options =>
//      {
//          options.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.RequireCertificate;

//      }));

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseBlocker();

//app.UseAuthorization();

//app.MapRazorPages();

var builder = WebApplication.CreateBuilder();

var services = builder.Services;

var configuration = builder.Configuration;

// Add services to the container.

services.AddClientCertificateFilter(configuration, "CertificateFilter");

//builder.WebHost.ConfigureKestrel(options => options.ConfigureHttpsDefaults(options =>
//{
//    options.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.RequireCertificate;

//}));
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
    await context.Response.WriteAsync("Successfuly executed");
    context.Response.StatusCode = 200;
});

app.Run();

public partial class Program { }