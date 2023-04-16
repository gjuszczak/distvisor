using Distvisor.App.Common.Interfaces;
using Distvisor.App.Core.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.EventLog.Commands.ReplayEvents
{
    public class ReplayEvents : Command
    {
        public string Name { get; set; }
    }

    public class ReplayEventsHandler : ICommandHandler<ReplayEvents>
    {
        private readonly IEventsReplayService _eventsReplayService;

        public ReplayEventsHandler(IEventsReplayService eventsReplayService)
        {
            _eventsReplayService = eventsReplayService;
        }

        public async Task<Guid> Handle(ReplayEvents command, CancellationToken cancellationToken)
        {
            await _eventsReplayService.ReplayAsync(cancellationToken);
            return command.CorrelationId;            
        }
    }
}
