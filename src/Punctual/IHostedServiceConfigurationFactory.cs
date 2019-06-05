using Microsoft.Extensions.Configuration;
using System;

namespace Punctual
{
    public interface IHostedServiceConfigurationFactory
    {
        IConfiguration GetConfiguration(Type HostedServiceType);
    }
}