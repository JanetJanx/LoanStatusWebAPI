namespace LoanStatusWebAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Loan> Loans { get; set; }
    }
}
