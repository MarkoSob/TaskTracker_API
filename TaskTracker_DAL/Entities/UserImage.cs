using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_DAL.Entities
{
    public class UserImage : Entity
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
