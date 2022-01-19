using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, object> _queryDispatchHelpers = new();

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            var queryType = query.GetType();
            var queryDispatchHelper = (IQueryDispatchHelper<TResult>)_queryDispatchHelpers.GetOrAdd(queryType, CreateQueryDispatchHelper<TResult>);
            return await queryDispatchHelper.Dispatch(_serviceProvider, query, cancellationToken);
        }

        private static IQueryDispatchHelper<TResult> CreateQueryDispatchHelper<TResult>(Type queryType)
        {
            var dispatchHelperType = typeof(QueryDispatchHelper<,>).MakeGenericType(queryType, typeof(TResult));
            return (IQueryDispatchHelper<TResult>)Activator.CreateInstance(dispatchHelperType);
        }

        private interface IQueryDispatchHelper<TResult>
        {
            Task<TResult> Dispatch(IServiceProvider serviceProvider, IQuery<TResult> query, CancellationToken cancellationToken);
        }

        private class QueryDispatchHelper<TQuery, TResult> : IQueryDispatchHelper<TResult>
            where TQuery : IQuery<TResult>
        {
            public async Task<TResult> Dispatch(IServiceProvider serviceProvider, IQuery<TResult> query, CancellationToken cancellationToken)
            {
                var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
                return await handler.Handle((TQuery)query, cancellationToken);
            }
        }
    }
}
