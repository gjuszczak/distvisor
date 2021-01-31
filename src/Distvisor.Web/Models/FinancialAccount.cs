using System;

namespace Distvisor.Web.Models
{
    public class FinancialAccount
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public FinancialAccountType Type { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
    }
}
