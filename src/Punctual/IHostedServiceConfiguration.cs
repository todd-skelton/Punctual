using Microsoft.Extensions.Configuration;

namespace Punctual
{
    public interface IHostedServiceConfiguration<T>
    {
        IConfiguration Configuration { get; }
    }
}