using Microsoft.Extensions.Hosting;

namespace Punctual
{
    public interface IHostedService<TScheduledAction> : IHostedService
    {
    }
}