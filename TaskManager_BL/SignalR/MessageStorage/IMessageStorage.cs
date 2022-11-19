using TaskTracker.Core;

namespace TaskTracker_BL.SignalR.MessageStorage
{
    public interface IMessageStorage
    {
        void Add(MessageSnapshot messageSnapshot);
        IEnumerable<MessageSnapshot> GetRecent(string callerNickname, int count = 50);
    }
}