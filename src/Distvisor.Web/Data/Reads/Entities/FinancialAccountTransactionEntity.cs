using Distvisor.Web.Models;

namespace Distvisor.Web.Data.Reads.Entities
{
    public class FinancialAccountTransactionEntity : FinancialAccountTransaction
    {
        public FinancialAccountEntity Account { get; set; }
    }
}
