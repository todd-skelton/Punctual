using Microsoft.Extensions.DependencyInjection;

namespace Punctual
{
    /// <summary>
    /// Interface for a builder used to configure Punctual
    /// </summary>
    public interface IPunctualBuilder
    {
        /// <summary>
        /// Service collection for Punctual
        /// </summary>
        IServiceCollection Services { get; }
    }
}