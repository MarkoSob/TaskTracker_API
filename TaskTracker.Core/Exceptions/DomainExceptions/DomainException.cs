using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
