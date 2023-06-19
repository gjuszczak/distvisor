using Distvisor.App.Core.Commands;

namespace Distvisor.App.Features.Redirections.Commands
{
    public abstract class RedirectionBaseCommand : Command
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
