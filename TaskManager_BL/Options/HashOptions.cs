namespace TaskTracker_BL.Options
{
    public class HashOptions
    {
        public string? Salt { get; set; }
        public int IterationCount { get; set; }
        public int NumBytesRequested { get; set; }
    }
}
