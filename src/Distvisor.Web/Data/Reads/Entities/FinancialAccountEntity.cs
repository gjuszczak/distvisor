using System.Collections.Generic;

namespace Distvisor.Web.Data.Reads.Entities
{
    public class FinancialAccountEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }

        public List<FinancialAccountPaycardEntity> Paycards { get; set; }
    }
}
