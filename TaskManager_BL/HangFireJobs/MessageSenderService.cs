using Microsoft.AspNetCore.SignalR;
using TaskTracker.Core;
using TaskTracker_BL.SignalR;

namespace TaskTracker_BL
{
    public class MessageSenderService : IMessageSenderService
    {
        private IHubContext<SignalRChatHub> _hubContext;
        public MessageSenderService(IHubContext<SignalRChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendRegularMessageToClients()
        {

            await _hubContext.Clients.All.SendAsync("GetMessage", new MessageSnapshot
            {
                IsPersonal = false,
                Message = "Message from Server",
                SenderUserInfo = new PublicUserInfo
                {
                    Nickname = "Server",
                    MessageColor = "Magenta"
                },
                SentDate = DateTime.Now.ToString("HH:mm")
            });
        }

        public async Task SendAlertMessageToClients()
        {
            await _hubContext.Clients.All.SendAsync("GetMessage", new MessageSnapshot
            {
                IsPersonal = false,
                Message = "ALERT!",
                SenderUserInfo = new PublicUserInfo
                {
                    MessageColor = "Red",
                    Nickname = "Server"
                },
                SentDate = DateTime.Now.ToString("HH:mm")
            });
        }
    }
}
