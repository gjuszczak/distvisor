namespace Distvisor.Web.Data.Reads.Entities
{
    public class FinancialAccountPaycardEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public FinancialAccountEntity Account { get; set; }
    }
}
