using Microsoft.AspNetCore.SignalR;
using Quartz;
using TaskTracker.Core;
using TaskTracker_BL.SignalR;

namespace TaskTracker_BL.Quartz_Jobs
{
    public class SendAlertMessageToAll : IJob
    {
        private IHubContext<SignalRChatHub> _hubContext;

        public SendAlertMessageToAll(IHubContext<SignalRChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task Execute(IJobExecutionContext context)
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
