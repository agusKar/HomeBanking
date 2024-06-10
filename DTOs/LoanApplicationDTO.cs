namespace HomeBanking.DTOs
{
    public class LoanApplicationDTO
    {
        public long loanId { get; set; }
        public double Amount { get; set; }
        public string Payments { get; set; }
        public string toAccountNumber { get; set; }
    }
}
