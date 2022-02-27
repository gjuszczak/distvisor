using Distvisor.App.Core.Dispatchers;

namespace Distvisor.App.Core.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
