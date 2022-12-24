using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Core.Exceptions.DataAccessExceptions
{
    public class ObjectNotFoundException : DataAccessException
    {
        public ObjectNotFoundException(Type objectType) : base(HttpStatusCode.NotFound, $"Object of type {objectType} not found")
        {

        }
    }
}
