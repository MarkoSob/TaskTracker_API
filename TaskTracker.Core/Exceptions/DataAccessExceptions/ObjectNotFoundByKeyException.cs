using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Core.Exceptions.DataAccessExceptions
{
    public class ObjectNotFoundByKeyException : DataAccessException
    {
        public ObjectNotFoundByKeyException(Type objectType, object key) : base(HttpStatusCode.NotFound, $"Object of type {objectType} with key={key} not found")
        {

        }
    }
}
