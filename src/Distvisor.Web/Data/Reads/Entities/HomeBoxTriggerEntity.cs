using Distvisor.Web.Models;
using System.Collections.Generic;

namespace Distvisor.Web.Data.Reads.Entities
{
    public class HomeBoxTriggerEntity : HomeBoxTrigger
    {
        public List<HomeBoxTriggerSourceEntity> Sources { get; set; }
        public List<HomeBoxTriggerTargetEntity> Targets { get; set; }
        public List<HomeBoxTriggerActionEntity> Actions { get; set; }
    }
}
