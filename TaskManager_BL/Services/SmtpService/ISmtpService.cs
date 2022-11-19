namespace TaskTracker_BL.Services.SmtpService
{
    public interface ISmtpService
    {
        Task SendEmailAsync(string email, string subject, string messageText);
    }
}
