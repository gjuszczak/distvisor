﻿using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Queries
{
    public interface IQueryHandler<in TQuery, TResult> 
        where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
