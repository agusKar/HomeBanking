using HomeBanking.Models;

namespace HomeBanking.DTOs
{
    public class AccountCreatedDTO
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
        public AccountCreatedDTO(Account account)
        {
            Number = account.Number;
            CreationDate = account.CreationDate;
            Balance = account.Balance;
        }
    }
}
