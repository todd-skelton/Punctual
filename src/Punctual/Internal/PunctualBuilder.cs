using Microsoft.Extensions.DependencyInjection;

namespace Punctual.Internal
{
    internal class PunctualBuilder : IPunctualBuilder
    {
        public PunctualBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
