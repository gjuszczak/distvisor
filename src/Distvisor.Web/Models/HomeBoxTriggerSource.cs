using System;

namespace Distvisor.Web.Models
{
    public class HomeBoxTriggerSource
    {
        public Guid Id { get; set; }
        public Guid TriggerId { get; set; }
        public HomeBoxTriggerSourceType Type { get; set; }
        public string MatchParam { get; set; }
    }
}
