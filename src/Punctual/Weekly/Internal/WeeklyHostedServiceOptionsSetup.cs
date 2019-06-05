using Microsoft.Extensions.Options;

namespace Punctual.Weekly.Internal
{
    internal class WeeklyHostedServiceOptionsSetup<TScheduledAction> : ConfigureFromConfigurationOptions<WeeklyHostedServiceOptions<TScheduledAction>>
where TScheduledAction : class, IScheduledAction
    {
        public WeeklyHostedServiceOptionsSetup(IHostedServiceConfiguration<WeeklyHostedService<TScheduledAction>> HostedServiceConfiguration)
            : base(HostedServiceConfiguration.Configuration)
        {
        }
    }
}
