using Distvisor.Web.Models;
using System;

namespace Distvisor.Web.Data.Reads.Entities
{
    public class FinancialAccountTransactionEntity : FinancialAccountTransaction
    {
        public Guid Id { get; set; }

        public FinancialAccountEntity Account { get; set; }
    }
}
