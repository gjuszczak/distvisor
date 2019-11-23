﻿using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public class DockerService : IDockerService
    {
        private readonly DockerClient client;

        public DockerService(IConfiguration config)
        {
            var uri = new Uri(config.GetConnectionString("Docker"));
            client = new DockerClientConfiguration(uri)
                .CreateClient();
        }

        public async Task<IEnumerable<string>> GetAllContainersAsync()
        {
            var containers = await client.Containers.ListContainersAsync(
                new ContainersListParameters() { Limit = 10 });

            return containers.SelectMany(x => x.Names).ToList();
        }
    }
}