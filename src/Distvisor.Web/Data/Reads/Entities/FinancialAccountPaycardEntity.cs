using System;

namespace Distvisor.Web.Data.Reads.Entities
{
    public class FinancialAccountPaycardEntity
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Name { get; set; }

        public FinancialAccountEntity Account { get; set; }
    }
}
