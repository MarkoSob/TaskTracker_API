namespace TaskTracker_BL.DTOs
{
    public class StartJobRequest
    {
        public string? JobTitle { get; set; }
        public int IntervalInSeconds { get; set; }
        public int RepeatCount { get; set; }
    }
}
