using Distvisor.App.Features.Redirections.Entities;
using System;

namespace Distvisor.App.Features.Redirections.Queries
{
    public class RedirectionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public static RedirectionDto FromEntity(RedirectionEntity entity)
        {
            return new RedirectionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Url = entity.Url,
            };
        }
    }
}
