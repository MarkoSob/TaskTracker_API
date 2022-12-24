namespace TaskTracker_BL.DTOs
{
    public class UserTaskDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
    }
}
