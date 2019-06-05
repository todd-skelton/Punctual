using Microsoft.Extensions.Options;

namespace Punctual
{
    /// <summary>
    /// Change token source used to trigger changes in the configuration
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="TSchduler"></typeparam>
    public class HostedServiceOptionsChangeTokenSource<TOptions, TSchduler> : ConfigurationChangeTokenSource<TOptions>
    {
        /// <summary>
        /// Initializes a new change token source
        /// </summary>
        /// <param name="HostedServiceConfiguration"></param>
        public HostedServiceOptionsChangeTokenSource(IHostedServiceConfiguration<TSchduler> HostedServiceConfiguration) : base(HostedServiceConfiguration.Configuration)
        {
        }
    }
}