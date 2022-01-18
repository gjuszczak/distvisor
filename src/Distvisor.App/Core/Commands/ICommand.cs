using MediatR;
using System;

namespace Distvisor.App.Core.Commands
{
    public interface ICommand : IRequest<Guid>
	{
		Guid Id { get; }
		Guid CorrelationId { get; }
		int ExpectedVersion { get; }
	}
}
