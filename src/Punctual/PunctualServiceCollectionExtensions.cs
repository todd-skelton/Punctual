using Microsoft.Extensions.DependencyInjection;
using Punctual.Internal;
using System;

namespace Punctual
{
    /// <summary>
    /// Extensions used to configure Punctual
    /// </summary>
    public static class PunctualServiceCollectionExtensions
    {
        /// <summary>
        /// Configures Punctual for a service collection. Typically this is called in ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddPunctual(this IServiceCollection services, Action<IPunctualBuilder> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddOptions();
            configure(new PunctualBuilder(services));

            return services;
        }
    }
}