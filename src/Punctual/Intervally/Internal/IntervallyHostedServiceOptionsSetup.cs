using Microsoft.Extensions.Options;

namespace Punctual.Intervally.Internal
{
    internal class IntervallyHostedServiceOptionsSetup<TScheduledAction> : ConfigureFromConfigurationOptions<IntervallyHostedServiceOptions<TScheduledAction>>
where TScheduledAction : class, IScheduledAction
    {
        public IntervallyHostedServiceOptionsSetup(IHostedServiceConfiguration<IntervallyHostedService<TScheduledAction>> HostedServiceConfiguration)
            : base(HostedServiceConfiguration.Configuration)
        {
        }
    }
}
