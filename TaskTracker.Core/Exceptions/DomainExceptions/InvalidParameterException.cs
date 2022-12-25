using System.Net;

namespace TaskTracker.Core.Exceptions.DomainExceptions
{
    public class InvalidParameterExeption : DomainException
    {
        public InvalidParameterExeption(object @object, string parameterName, string message)
            : base(HttpStatusCode.BadRequest, $"{message} \nparameter '{parameterName}' has invalid value", @object)
        {
        }
    }
}
