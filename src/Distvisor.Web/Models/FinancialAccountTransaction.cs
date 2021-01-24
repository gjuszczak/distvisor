using System;

namespace Distvisor.Web.Models
{
    public class FinancialAccountTransaction
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public long SeqNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PostingDate { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public FinancialAccountTransactionSource Source { get; set; }
        public string TransactionHash { get; set; }
    }
}
