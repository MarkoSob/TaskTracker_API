﻿using System.ComponentModel.DataAnnotations;

namespace TaskTracker_BL.DTOs
{
    public class UserProfileDataDto
    {
        public Guid Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? CreationDate { get; set; }
    }
}
