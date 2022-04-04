using Distvisor.App.Core.Entities;
using Distvisor.App.HomeBox.Enums;
using Distvisor.App.HomeBox.ValueObjects;
using System;

namespace Distvisor.App.HomeBox.Entities
{
    public class GatewaySessionEntity : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public GatewayToken Token { get; set; }
        public GatewaySessionStatus Status { get; set; }
    }
}
