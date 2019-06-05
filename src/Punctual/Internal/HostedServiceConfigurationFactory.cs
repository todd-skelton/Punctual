using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Punctual.Internal
{
    internal class HostedServiceConfigurationFactory : IHostedServiceConfigurationFactory
    {
        private readonly IEnumerable<PunctualConfiguration> _configurations;

        public HostedServiceConfigurationFactory(IEnumerable<PunctualConfiguration> configurations)
        {
            _configurations = configurations;
        }

        public IConfiguration GetConfiguration(Type HostedServiceType)
        {
            if (HostedServiceType == null)
            {
                throw new ArgumentNullException(nameof(HostedServiceType));
            }

            Type scheduledActionType = HostedServiceType.GetGenericArguments()[0];

            var scheduledActionAlias = ScheduledActionAliasUtilities.GetAlias(scheduledActionType);

            var configurationBuilder = new ConfigurationBuilder();
            foreach (var configuration in _configurations)
            {
                var sectionFromName = configuration.Configuration.GetSection(scheduledActionAlias);
                configurationBuilder.AddConfiguration(sectionFromName);
            }
            return configurationBuilder.Build();
        }
    }
}
