using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Punctual.Internal;
using Punctual.Intervally;
using Punctual.Intervally.Internal;
using Punctual.Weekly;
using Punctual.Weekly.Internal;
using System;

namespace Punctual
{
    public static class PunctualBuilderExtensions
    {
        private static void AddConfiguration(this IPunctualBuilder builder)
        {
            builder.Services.TryAddSingleton<IHostedServiceConfigurationFactory, HostedServiceConfigurationFactory>();
            builder.Services.TryAddSingleton(typeof(IHostedServiceConfiguration<>), typeof(HostedServiceConfiguration<>));
        }

        public static IPunctualBuilder AddConfiguration(this IPunctualBuilder builder, IConfiguration configuration)
        {
            builder.AddConfiguration();
            builder.Services.AddSingleton(new PunctualConfiguration(configuration.GetSection("Punctual")));

            return builder;
        }


        public static IPunctualBuilder Add<THostedService, THostedServiceOptions, THostedServiceOptionsSetup, TScheduledAction>(this IPunctualBuilder builder)
            where THostedService : class, IHostedService<TScheduledAction>
            where THostedServiceOptions : class, IHostedServiceOptions
            where THostedServiceOptionsSetup : class, IConfigureOptions<THostedServiceOptions>
            where TScheduledAction : class, IScheduledAction
        {
            builder.AddConfiguration();

            builder.Services.TryAddSingleton<TScheduledAction>();
            builder.Services.TryAddSingleton<THostedService>();
            builder.Services.TryAddSingleton<IHostedService<TScheduledAction>, THostedService>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, THostedService>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<THostedServiceOptions>, THostedServiceOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<THostedServiceOptions>, HostedServiceOptionsChangeTokenSource<THostedServiceOptions, THostedService>>());

            return builder;
        }

        public static IPunctualBuilder Add<THostedService, THostedServiceOptions, THostedServiceOptionsSetup, TScheduledAction>(this IPunctualBuilder builder, Action<THostedServiceOptions> configure)
            where THostedService : class, IHostedService<TScheduledAction>
            where THostedServiceOptions : class, IHostedServiceOptions
            where THostedServiceOptionsSetup : class, IConfigureOptions<THostedServiceOptions>
            where TScheduledAction : class, IScheduledAction
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.Add<THostedService, THostedServiceOptions, THostedServiceOptionsSetup, TScheduledAction>();
            builder.Services.Configure(configure);

            return builder;
        }

        public static IPunctualBuilder AddWeekly<TScheduledAction>(this IPunctualBuilder builder)
            where TScheduledAction : class, IScheduledAction
        {
            return builder.Add<WeeklyHostedService<TScheduledAction>, WeeklyHostedServiceOptions<TScheduledAction>, WeeklyHostedServiceOptionsSetup<TScheduledAction>, TScheduledAction>();
        }

        public static IPunctualBuilder AddWeekly<TScheduledAction>(this IPunctualBuilder builder, Action<WeeklyHostedServiceOptions<TScheduledAction>> configure)
            where TScheduledAction : class, IScheduledAction
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddWeekly<TScheduledAction>();
            builder.Services.Configure(configure);

            return builder;
        }

        public static IPunctualBuilder AddIntervally<TScheduledAction>(this IPunctualBuilder builder)
            where TScheduledAction : class, IScheduledAction
        {
            return builder.Add<IntervallyHostedService<TScheduledAction>, IntervallyHostedServiceOptions<TScheduledAction>, IntervallyHostedServiceOptionsSetup<TScheduledAction>, TScheduledAction>();
        }

        public static IPunctualBuilder AddIntervally<TScheduledAction>(this IPunctualBuilder builder, Action<IntervallyHostedServiceOptions<TScheduledAction>> configure)
            where TScheduledAction : class, IScheduledAction
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddIntervally<TScheduledAction>();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}