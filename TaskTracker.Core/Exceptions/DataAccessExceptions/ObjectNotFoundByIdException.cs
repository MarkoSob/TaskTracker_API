using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Core.Exceptions.DataAccessExceptions
{
    public class ObjectNotFoundByIdException : DataAccessException
    {
        public ObjectNotFoundByIdException(Type objectType, Guid id) : base(HttpStatusCode.NotFound, $"Object of type {objectType} with id={id} not found")
        {

        }
    }
}
