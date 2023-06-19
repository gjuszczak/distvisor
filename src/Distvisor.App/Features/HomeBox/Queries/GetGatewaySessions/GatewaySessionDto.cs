using Distvisor.App.Features.HomeBox.Entities;
using Distvisor.App.Features.HomeBox.Enums;
using System;

namespace Distvisor.App.Features.HomeBox.Queries.GetGatewaySessions
{
    public class GatewaySessionDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset TokenGeneratedAt { get; set; }
        public GatewaySessionStatus Status { get; set; }

        public static GatewaySessionDto FromEntity(GatewaySessionEntity entity)
        {
            return new GatewaySessionDto
            {
                Id = entity.Id,
                TokenGeneratedAt = entity.Token.GeneratedAt,
                Status = entity.Status,
            };
        }
    }
}
