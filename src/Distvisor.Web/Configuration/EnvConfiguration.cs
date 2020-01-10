using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Configuration
{
    public class EnvConfiguration
    {
        public string GithubApiToken { get; set; }
        public string GithubRepoOwner { get; set; }
        public string GithubRepoName { get; set; }
        public string CurrentVersion { get; set; }
    }
}
