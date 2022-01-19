using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Bili.Extensions.RequestBlocker.Tests
{
    [TestClass]
    public class WebTests
    {

        [TestMethod]
        public async Task TestMethod1()
        {
            var builder = WebApplication.CreateBuilder();

            var services = builder.Services;

            var configuration = builder.Configuration;

            // Add services to the container.

            services.AddClientCertificateFilter(configuration, "CertificateFilter");

            builder.WebHost.ConfigureKestrel(options => options.ConfigureHttpsDefaults(options =>
            {
                options.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.RequireCertificate;
                options.AllowAnyClientCertificate();

            }));

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseBlocker();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Successfuly executed");
                context.Response.StatusCode = 200;
            });

            await app.StartAsync();

            try
            {
                string certPath = Path.Combine(@"C:\temp", "kv-ontimev2-apim-webapi-20220106.pfx");
                var cert = new X509Certificate2(certPath);
                var handler = new HttpClientHandler();

                handler.ClientCertificates.Add(cert);
                handler.ServerCertificateCustomValidationCallback = (request, certificate, chain, sslErrors) => true;

                var client = new HttpClient(handler);

                string baseAddress = app.Urls.First(url => url.StartsWith("https"));
                client.BaseAddress = new Uri(baseAddress);
                
                client.DefaultRequestHeaders.ConnectionClose = true;

                var response = await client.GetAsync("/");
                Assert.IsNotNull(response);
                Assert.IsTrue(response.IsSuccessStatusCode);
            }
            finally
            {
                await app.StopAsync();
            }

        }
    }
}