namespace TaskTracker_BL.Services.SmtpService
{
    public class MockSmtpService : ISmtpService
    {
        public Task SendEmailAsync(string email, string subject, string messageText)
        {
            return Task.CompletedTask;
        }
    }
}
