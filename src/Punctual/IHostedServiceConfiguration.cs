using Microsoft.Extensions.Configuration;

namespace Punctual
{
    /// <summary>
    /// Interface that holds configurations for a hosted service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHostedServiceConfiguration<T>
    {
        /// <summary>
        /// Configuration
        /// </summary>
        IConfiguration Configuration { get; }
    }
}