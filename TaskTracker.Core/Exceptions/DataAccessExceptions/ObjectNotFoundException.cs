using System.Net;

namespace TaskTracker.Core.Exceptions.DataAccessExceptions
{
    public class ObjectNotFoundException : DataAccessException
    {
        public ObjectNotFoundException(Type objectType) : base(HttpStatusCode.NotFound, $"Object of type {objectType} not found")
        {

        }
    }
}
