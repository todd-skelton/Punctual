using Microsoft.Extensions.DependencyInjection;

namespace Punctual
{
    public interface IPunctualBuilder
    {
        IServiceCollection Services { get; }
    }
}