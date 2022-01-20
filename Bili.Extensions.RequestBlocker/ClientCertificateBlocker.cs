using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bili.Extensions.RequestBlocker
{
    internal class ClientCertificateBlocker : IRequestBlocker
    {
        private readonly IOptions<ClientCertificateBlockerConfiguration> clientCertificateBlockerConfiguration;

        public ClientCertificateBlocker(IOptions<ClientCertificateBlockerConfiguration> clientCertificateBlockerConfiguration)
        {
            this.clientCertificateBlockerConfiguration = clientCertificateBlockerConfiguration;
        }

        public Task<bool> AllowRequestAsync(HttpContext context)
        {
            var certificate = context.Connection.ClientCertificate;
            if (certificate == null)
                return Task.FromResult(false);
            if (clientCertificateBlockerConfiguration.Value.VerifyCertificate)
            {
                if (certificate.Verify() == false)
                {
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(clientCertificateBlockerConfiguration.Value.AllowedThumbprints.Contains(certificate.Thumbprint));
        }
    }
}
