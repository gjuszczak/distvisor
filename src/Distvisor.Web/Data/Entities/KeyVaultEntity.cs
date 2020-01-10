using Distvisor.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Entities
{
    public class KeyVaultEntity
    {
        public KeyType Id { get; set; }
        public string KeyValue { get; set; }
    }
}
