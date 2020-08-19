using System.Threading;
using System.Threading.Tasks;

namespace Punctual
{
    /// <summary>
    /// Interface used to implement a scheduled action that can be performed by a hosted service
    /// </summary>
    public interface IScheduledAction
    {
        /// <summary>
        /// The action to perform
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Execute(CancellationToken cancellationToken);
    }
}