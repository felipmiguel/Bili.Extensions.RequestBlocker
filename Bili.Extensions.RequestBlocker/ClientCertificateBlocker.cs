using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Bili.Extensions.RequestBlocker
{
    internal class ClientCertificateBlocker : IRequestBlocker
    {
        private readonly IOptions<ClientCertificateBlockerConfiguration> clientCertificateBlockerConfiguration;
        private readonly ILogger<ClientCertificateBlocker> logger;

        public ClientCertificateBlocker(IOptions<ClientCertificateBlockerConfiguration> clientCertificateBlockerConfiguration, 
            ILogger<ClientCertificateBlocker> logger)
        {
            this.clientCertificateBlockerConfiguration = clientCertificateBlockerConfiguration;
            this.logger = logger;
        }

        public Task<bool> AllowRequestAsync(HttpContext context)
        {
            var certificate = context.Connection.ClientCertificate;
            if (certificate == null)
            {
                logger.LogInformation("No client certificate present");
                return Task.FromResult(false);
            }
            if (clientCertificateBlockerConfiguration.Value.VerifyCertificate)
            {
                if (certificate.Verify() == false)
                {
                    logger.LogInformation("Client certificate verification failed");
                    return Task.FromResult(false);
                }
            }
            bool result = clientCertificateBlockerConfiguration.Value.AllowedThumbprints.Contains(certificate.Thumbprint);
            if (!result)
            {
                logger.LogInformation("Client certificate thumbprint not allowed. {0}", certificate.Thumbprint);
            }
            return Task.FromResult(result);
        }
    }
}
