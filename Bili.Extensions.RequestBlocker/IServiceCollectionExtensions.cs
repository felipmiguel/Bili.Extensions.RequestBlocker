using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bili.Extensions.RequestBlocker
{
    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection AddClientCertificateFilter(this IServiceCollection services, Action<ClientCertificateBlockerConfiguration> options)
        {
            return services
                .Configure(options)
                .AddSingleton<IRequestBlocker, ClientCertificateBlocker>();
        }

        public static IServiceCollection AddClientCertificateFilter(this IServiceCollection services, IConfiguration configuration, string sectionName)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            return services.AddClientCertificateFilter(configuration.GetSection(sectionName));
            

        }

        public static IServiceCollection AddClientCertificateFilter(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            if (configurationSection is null)
            {
                throw new ArgumentNullException(nameof(configurationSection));
            }

            return services
                .AddClientCertificateFilter((config) => configurationSection.Bind(config));
        }
    }
}
