using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Dispatchers
{
    public class DispatchWrapper<TRequest, TResult, THandler> : IDispatchWrapper<TResult>
        where TRequest : IRequest<TResult>
        where THandler : IHandler<TRequest, TResult>
    {
        private readonly IPipelineProvider _pipelineProvider;

        public DispatchWrapper(IPipelineProvider pipelineProvider)
        {
            _pipelineProvider = pipelineProvider;
        }

        public async Task<TResult> Dispatch(IServiceProvider serviceProvider, IRequest<TResult> request, CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetRequiredService<THandler>();
            async Task<TResult> Handler() => await handler.Handle((TRequest)request, cancellationToken);
            return await _pipelineProvider.Execute(serviceProvider, Handler, (TRequest)request, cancellationToken);
        }
    }
}
