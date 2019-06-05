using Microsoft.Extensions.Hosting;

namespace Punctual
{
    /// <summary>
    /// Interface for a hosted service that can perform an action
    /// </summary>
    /// <typeparam name="TScheduledAction"></typeparam>
    public interface IHostedService<TScheduledAction> : IHostedService
    {
    }
}