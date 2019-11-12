using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Models
{
    public class Session
    {
        public string Id { get; set; }
        public DateTime IssuedAtUtc { get; set; }
        public DateTime ExpireOnUtc { get; set; }

        public User User { get; set; }
    }
}
