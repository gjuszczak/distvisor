using Docker.DotNet;
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

        public async Task UpdateImageAsync(string tag)
        {
            var imgParams = new ImagesCreateParameters
            {
                FromImage = "test",
                Tag = tag
            };
            var auth = new AuthConfig();
            var progress = new DockerProgress(); 

            await client.Images.CreateImageAsync(imgParams, auth, progress);
        }
    }

    public class DockerProgress : IProgress<JSONMessage>
    {
        public void Report(JSONMessage value)
        {
            // do nothing
        }
    }
}
