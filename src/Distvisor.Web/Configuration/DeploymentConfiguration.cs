namespace Distvisor.Web.Configuration
{
    public class DeploymentConfiguration
    {
        public string CurrentVersion { get; set; }
        public string[] Environments { get; set; }
        public string Repository { get; set; }
        public string ApiKey { get; set; }
        public string Workflow { get; set; }
        public string Branch { get; set; }
    }
}
