using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public class GithubService : IGithubService
    {
        private readonly string appName = "distvisor";
        private readonly string repoName = "distvisor";
        private readonly string repoOwner = "gjuszczak";

        public async Task<IEnumerable<string>> GetReleasesAsync()
        {
            var client = new GitHubClient(new ProductHeaderValue(appName));
            var releases = await client.Repository.Release.GetAll(repoOwner, repoName);
            return releases.Select(x => x.TagName).ToList();
        }
    }
}
