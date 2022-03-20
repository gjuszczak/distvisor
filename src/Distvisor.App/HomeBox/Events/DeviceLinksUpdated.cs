using Distvisor.App.Core.Events;
using Distvisor.App.HomeBox.ValueObjects;
using System.Collections.Generic;

namespace Distvisor.App.HomeBox.Events
{
    public class DeviceLinksUpdated : Event
    {
        public IEnumerable<DeviceLink> Added { get; init; }
        public IEnumerable<DeviceLink> Deleted { get; init; }

        public DeviceLinksUpdated(IEnumerable<DeviceLink> added, IEnumerable<DeviceLink> deleted)
        {
            Added = added;
            Deleted = deleted;
        }
    }
}
