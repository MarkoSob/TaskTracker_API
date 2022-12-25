using System.Net;
using TaskTracker.Core.Exceptions.Exceptions;

namespace TaskTracker.Core.Exceptions.DataAccessExceptions
{
    public class DataAccessException : HttpResponseException
	{
		public DataAccessException(HttpStatusCode statusCode, string message = "", object @object = null) : base(statusCode, message, @object)
		{
		}
	}
}
