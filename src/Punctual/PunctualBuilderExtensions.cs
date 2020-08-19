using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Punctual.Internal;
using Punctual.Periodically;
using Punctual.Periodically.Internal;
using Punctual.Weekly;
using Punctual.Weekly.Internal;
using System;

namespace Punctual
{
    /// <summary>
    /// Extensions used to configure Punctual
    /// </summary>
    public static class PunctualBuilderExtensions
    {
        private static void AddConfiguration(this IPunctualBuilder builder)
        {
            builder.Services.TryAddSingleton<IScheduledActionConfigurationFactory, HostedServiceConfigurationFactory>();
            builder.Services.TryAddSingleton(typeof(IHostedServiceConfiguration<>), typeof(HostedServiceConfiguration<>));
        }

        /// <summary>
        /// Adds the configuration used to configure Punctual
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IPunctualBuilder AddConfiguration(this IPunctualBuilder builder, IConfiguration configuration)
        {
            builder.AddConfiguration();
            builder.Services.AddSingleton(new PunctualConfiguration(configuration.GetSection("Punctual")));

            return builder;
        }

        /// <summary>
        /// Adds a new hosted service with options and the action to perform
        /// </summary>
        /// <typeparam name="THostedService"></typeparam>
        /// <typeparam name="THostedServiceOptions"></typeparam>
        /// <typeparam name="THostedServiceOptionsSetup"></typeparam>
        /// <typeparam name="TScheduledAction"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IPunctualBuilder Add<THostedService, THostedServiceOptions, THostedServiceOptionsSetup, TScheduledAction>(this IPunctualBuilder builder)
            where THostedService : class, IHostedService<TScheduledAction>
            where THostedServiceOptions : class, IHostedServiceOptions
            where THostedServiceOptionsSetup : class, IConfigureOptions<THostedServiceOptions>
            where TScheduledAction : class, IScheduledAction
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<THostedServiceOptions>, THostedServiceOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<THostedServiceOptions>, HostedServiceOptionsChangeTokenSource<THostedServiceOptions, THostedService>>());
            builder.Services.TryAddTransient<TScheduledAction>();
            builder.Services.AddHostedService<THostedService>();

            return builder;
        }

        /// <summary>
        /// Adds a new hosted service with options and the action to perform
        /// </summary>
        /// <typeparam name="THostedService"></typeparam>
        /// <typeparam name="THostedServiceOptions"></typeparam>
        /// <typeparam name="THostedServiceOptionsSetup"></typeparam>
        /// <typeparam name="TScheduledAction"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds a hosted service that will perform actions on a weekly schedule
        /// </summary>
        /// <typeparam name="TScheduledAction"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IPunctualBuilder AddWeekly<TScheduledAction>(this IPunctualBuilder builder)
            where TScheduledAction : class, IScheduledAction
        {
            return builder.Add<WeeklyHostedService<TScheduledAction>, WeeklyHostedServiceOptions<TScheduledAction>, WeeklyHostedServiceOptionsSetup<TScheduledAction>, TScheduledAction>();
        }

        /// <summary>
        /// Adds a hosted service that will perform actions on a weekly schedule
        /// </summary>
        /// <typeparam name="TScheduledAction"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds a hosted service that will perform actions on an interval
        /// </summary>
        /// <typeparam name="TScheduledAction"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IPunctualBuilder AddPeriodically<TScheduledAction>(this IPunctualBuilder builder)
            where TScheduledAction : class, IScheduledAction
        {
            return builder.Add<PeriodicallyHostedService<TScheduledAction>, PeriodicallyHostedServiceOptions<TScheduledAction>, PeriodicallyHostedServiceOptionsSetup<TScheduledAction>, TScheduledAction>();
        }

        /// <summary>
        /// Adds a hosted service that will perform actions on an interval
        /// </summary>
        /// <typeparam name="TScheduledAction"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IPunctualBuilder AddPeriodically<TScheduledAction>(this IPunctualBuilder builder, Action<PeriodicallyHostedServiceOptions<TScheduledAction>> configure)
            where TScheduledAction : class, IScheduledAction
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddPeriodically<TScheduledAction>();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}