using Distvisor.App.HomeBox.Entities;
using System;

namespace Distvisor.App.HomeBox.Queries.GetGatewaySessions
{
    public class GatewaySessionDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset TokenGeneratedAt { get; set; }
        public string Status { get; set; }
        
        public static GatewaySessionDto FromEntity(GatewaySessionEntity entity)
        {
            return new GatewaySessionDto
            {
                Id = entity.Id,
                TokenGeneratedAt = entity.Token.GeneratedAt,
                Status = entity.Status.Name,
            };
        }
    }
}
