using System;

namespace Distvisor.Web.Models
{
    public class HomeBoxTriggerTarget
    {
        public Guid Id { get; set; }
        public Guid TriggerId { get; set; }
        public string DeviceIdentifier { get; set; }
    }
}
