using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Core.QueryParameters
{
    public class QueryParameters<T>
    {
        public string? OrderBy { get; set; } = "Title";
        public string? SearchTerm { get; set; }
    }
}
