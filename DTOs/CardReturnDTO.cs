namespace HomeBanking.DTOs
{
    public class CardReturnDTO
    {
        public CardDTO Card { get; set; }
        public string MessageStatusCode { get; set; }
        public int StatusCode { get; set; }
    }
}
