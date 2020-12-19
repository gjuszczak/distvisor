using Distvisor.Web.Models;
using System;
using System.Collections.Generic;

namespace Distvisor.Web.Data.Reads.Entities
{
    public class FinancialAccountEntity : FinancialAccount
    {
        public Guid Id { get; set; }

        public List<FinancialAccountPaycardEntity> Paycards { get; set; }
        public List<FinancialAccountTransactionEntity> Transactions { get; set; }
    }
}
