﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_BL.DTOs
{
    public class UserData
    {
        public Guid Id { get; set; }
        public string? AccessToken { get; set; }
    }
}
