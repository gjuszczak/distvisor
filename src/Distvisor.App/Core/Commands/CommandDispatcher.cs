using Distvisor.App.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, ICommandDispatchHelper> _commandDispatchHelpers = new();

        public CommandDispatcher(ICorrelationIdProvider correlationIdProvider, IServiceProvider serviceProvider)
        {
            _correlationIdProvider = correlationIdProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<Guid> DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            _correlationIdProvider.SetCorrelationId(command.CorrelationId);
            var commandType = command.GetType();
            var commandDispatchHelper = _commandDispatchHelpers.GetOrAdd(commandType, CreateCommandDispatchHelper);
            return await commandDispatchHelper.Dispatch(_serviceProvider, command, cancellationToken);
        }

        private static ICommandDispatchHelper CreateCommandDispatchHelper(Type commandType)
        {
            var dispatchHelperType = typeof(CommandDispatchHelper<>).MakeGenericType(commandType);
            return (ICommandDispatchHelper)Activator.CreateInstance(dispatchHelperType);
        }

        private interface ICommandDispatchHelper
        {
            Task<Guid> Dispatch(IServiceProvider serviceProvider, ICommand command, CancellationToken cancellationToken);
        }

        private class CommandDispatchHelper<TCommand> : ICommandDispatchHelper
            where TCommand : ICommand
        {
            public async Task<Guid> Dispatch(IServiceProvider serviceProvider, ICommand command, CancellationToken cancellationToken)
            {
                var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
                return await handler.Handle((TCommand)command, cancellationToken);
            }
        }
    }
}
