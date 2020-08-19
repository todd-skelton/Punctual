using Microsoft.Extensions.Options;

namespace Punctual.Periodically.Internal
{
    internal class PeriodicallyHostedServiceOptionsSetup<TScheduledAction> : ConfigureFromConfigurationOptions<PeriodicallyHostedServiceOptions<TScheduledAction>>
where TScheduledAction : class, IScheduledAction
    {
        public PeriodicallyHostedServiceOptionsSetup(IHostedServiceConfiguration<PeriodicallyHostedService<TScheduledAction>> HostedServiceConfiguration)
            : base(HostedServiceConfiguration.Configuration)
        {
        }
    }
}
