using System.Net;

namespace TaskTracker.Core.Exceptions.DataAccessExceptions
{
    public class ObjectNotFoundByIdException : DataAccessException
    {
        public ObjectNotFoundByIdException(Type objectType, Guid id) : base(HttpStatusCode.NotFound, $"Object of type {objectType} with id={id} not found")
        {

        }
    }
}
