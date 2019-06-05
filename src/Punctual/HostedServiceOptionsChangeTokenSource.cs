using Microsoft.Extensions.Options;

namespace Punctual
{
    public class HostedServiceOptionsChangeTokenSource<TOptions, TSchduler> : ConfigurationChangeTokenSource<TOptions>
    {
        public HostedServiceOptionsChangeTokenSource(IHostedServiceConfiguration<TSchduler> HostedServiceConfiguration) : base(HostedServiceConfiguration.Configuration)
        {
        }
    }
}