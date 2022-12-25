using System.Net;
using TaskTracker.Core.Exceptions.Exceptions;

namespace TaskTracker.Core.Exceptions.DomainExceptions
{
    public class DomainException : HttpResponseException
	{
		public DomainException(HttpStatusCode statusCode, string message = "", object @object = null) : base(statusCode, message, @object)
		{
		}
	}
}
