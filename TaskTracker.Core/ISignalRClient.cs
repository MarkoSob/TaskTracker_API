namespace TaskTracker.Core
{
    public interface ISignalRClient
    {
        Task GetMessage(MessageSnapshot messageSnapshot);
    }
}