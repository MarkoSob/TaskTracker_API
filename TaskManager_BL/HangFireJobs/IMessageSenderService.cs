
namespace TaskTracker_BL
{
    public interface IMessageSenderService
    {
        Task SendAlertMessageToClients();
        Task SendRegularMessageToClients();
    }
}