using System.Threading;
using System.Threading.Tasks;

namespace Punctual
{
    public interface IScheduledAction
    {
        Task Action(CancellationToken cancellationToken);
    }
}