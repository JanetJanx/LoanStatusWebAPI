namespace LoanStatusWebAPI.Models
{
    public class Logtrails
    {
        public int Id { get; set; }
        public string EndPoint { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string ActionbY { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
