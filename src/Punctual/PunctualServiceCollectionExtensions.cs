using Microsoft.Extensions.DependencyInjection;
using Punctual.Internal;
using System;

namespace Punctual
{
    public static class PunctualServiceCollectionExtensions
    {
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