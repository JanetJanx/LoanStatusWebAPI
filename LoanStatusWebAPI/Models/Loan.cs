namespace LoanStatusWebAPI.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime DisbursementDate { get; set; }
        public int LoanPeriod{ get; set; }
        public decimal OutstandingAmount { get; set; }
    }
}
