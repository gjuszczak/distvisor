using Distvisor.App.Core.Dispatchers;

namespace Distvisor.App.Core.Queries
{
    public interface IQueryHandler<in TQuery, TResult> : IHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}
