using System;

namespace Distvisor.App.Core.Commands
{
    public interface ICommand
	{
		Guid Id { get; }
		Guid CorrelationId { get; }
		int ExpectedVersion { get; }
	}
}
