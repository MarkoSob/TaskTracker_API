using System.Net;

namespace TaskTracker.Core.Exceptions.DomainExceptions
{
    public class NullIdException : DomainException
    {
        public NullIdException() : base(HttpStatusCode.BadRequest, "Can`t find object when id is null")
        {
        }
    }
}
