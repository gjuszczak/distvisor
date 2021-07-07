using System;
using System.Text.Json;

namespace Distvisor.Web.Models
{
    public class HomeBoxTriggerAction
    {
        public Guid Id { get; set; }
        public Guid TriggerId { get; set; }
        public int? LastExecutedActionNumber { get; set; }
        public int? LastExecutedActionMinDelayMs { get; set; }
        public int? LastExecutedActionMaxDelayMs { get; set; }
        public bool? IsDeviceOn { get; set; }
        public JsonElement Payload { get; set; }
    }
}
