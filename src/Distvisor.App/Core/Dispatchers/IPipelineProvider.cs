using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Dispatchers
{
    public interface IPipelineProvider
    {
        Task<TResponse> Execute<TRequest, TResponse>(
            IServiceProvider serviceProvider, 
            RequestHandlerDelegate<TResponse> handler, 
            TRequest request, 
            CancellationToken cancellationToken) 
            where TRequest : IRequest<TResponse>;
    }
}