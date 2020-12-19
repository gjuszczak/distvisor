using System;

namespace Distvisor.Web.Models
{
    public class FinancialAccountTransaction
    {
        public Guid AccountId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Details { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public bool IsBalanceEstimated { get; set; }
        public FinancialAccountTransactionDataSource DataSource { get; set; }
    }
}
