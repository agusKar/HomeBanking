using System.Transactions;

namespace HomeBanking.DTOs
{
    public class TransferReturnDTO
    {
        public ICollection<TransactionDTO> Transactions { get; set; }
        public string MessageStatusCode { get; set; }
        public int StatusCode { get; set; }
    }
}
