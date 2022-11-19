using System.Collections.Concurrent;
using TaskTracker.Core;

namespace TaskTracker_BL.SignalR.MessageStorage
{
    public class MessageStorage : IMessageStorage
    {
        private readonly ConcurrentQueue<MessageSnapshot> _messageStorage;
        const int defaultNumber = 50;

        public MessageStorage()
        {
            _messageStorage = new ConcurrentQueue<MessageSnapshot>();
        }

        public void Add(MessageSnapshot messageSnapshot)
        {
            _messageStorage.Enqueue(messageSnapshot);
        }

        public IEnumerable<MessageSnapshot> GetRecent(
            string callerNickname,
            int count = defaultNumber)
        {
            var messageArray = _messageStorage.Where(
                x => !x.IsPersonal ||
                x.ReceiverNickname == callerNickname ||
                x.SenderUserInfo!.Nickname == callerNickname).ToArray();

            return messageArray.Take(count);
        }
    }
}
