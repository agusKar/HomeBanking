using System.Net;

namespace HomeBanking.Utilities
{
    public class CustomException : Exception
    {
        public HttpStatusCode StatusCode;
        public CustomException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
