using Distvisor.App.Core.ValueObjects;
using System;
using System.Collections.Generic;

namespace Distvisor.App.HomeBox.ValueObjects
{
    public class DeviceLink : ValueObject
    {
        public DeviceLink(Guid deviceAggregateId, string deviceGatewayId)
        {
            DeviceAggregateId = deviceAggregateId;
            DeviceGatewayId = deviceGatewayId;
        }

        public Guid DeviceAggregateId { get; init; }
        public string DeviceGatewayId { get; init; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DeviceAggregateId;
            yield return DeviceGatewayId;
        }
    }
}
