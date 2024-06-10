using System.Net;

namespace HomeBanking.Utilities
{
    public class CustomException : Exception
    {
        public int StatusCode;
        public CustomException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
