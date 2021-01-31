using Distvisor.Web.Models;
using System.Collections.Generic;

namespace Distvisor.Web.Data.Reads.Entities
{
    public class FinancialAccountEntity : FinancialAccount
    {
        public List<FinancialAccountTransactionEntity> Transactions { get; set; }
    }
}
