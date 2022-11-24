﻿namespace TaskTracker_BL.DTOs
{
    public class UserTaskDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public string? Status { get; set; }

    }
}
