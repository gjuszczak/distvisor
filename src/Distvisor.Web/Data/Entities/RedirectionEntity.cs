using System.ComponentModel.DataAnnotations;

namespace Distvisor.Web.Data.Entities
{
    public class RedirectionEntity
    {
        [Key]
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
