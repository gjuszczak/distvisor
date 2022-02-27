using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Dispatchers
{
    public class PipelineProvider : IPipelineProvider
    {
        public async Task<TResponse> Execute<TRequest, TResponse>(
            IServiceProvider serviceProvider, 
            RequestHandlerDelegate<TResponse> handler, 
            TRequest request, 
            CancellationToken cancellationToken)
            where TRequest : IRequest<TResponse>
        {
            var pipeline = serviceProvider
                .GetServices<IPipelineBehaviour<TRequest, TResponse>>()
                .Reverse();

            if (pipeline.Any())
            {
                return await pipeline.Aggregate(handler, (next, pipeline) =>
                    () => pipeline.Handle(request, next, cancellationToken))();
            }
            else
            {
                return await handler();
            }
        }
    }
}
