using Distvisor.App.Core.Entities;
using System;

namespace Distvisor.App.HomeBox.Entities
{
    public class GatewaySessionEntity : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
    }
}
