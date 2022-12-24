using System.Net;

namespace TaskTracker.Core.Exceptions.Exceptions
{
    public class HttpResponseException : Exception
    {
        public int StatusCode { get; }
        public object Object { get; }

        protected HttpResponseException(HttpStatusCode statusCode, string message = "", object @object = null)
            : base(message)
        {
            StatusCode = (int)statusCode;
            Object = @object;
        }

        public override string Message
            => base.Message + $"\nError code: {StatusCode}";
    }
}
