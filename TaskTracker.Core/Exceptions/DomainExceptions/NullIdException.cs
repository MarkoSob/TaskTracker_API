using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Core.Exceptions.DomainExceptions
{
    public class NullIdException : DomainException
    {
        public NullIdException() : base(HttpStatusCode.BadRequest, "Can`t find object when id is null")
        {
        }
    }
}
