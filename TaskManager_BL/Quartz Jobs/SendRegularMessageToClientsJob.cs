using Microsoft.AspNetCore.SignalR;
using Quartz;
using TaskTracker.Core;
using TaskTracker_BL.SignalR;

namespace TaskTracker_BL.Quartz_Jobs
{
    public class SendRegularMessageToClientsJob : IJob
    {
        private IHubContext<SignalRChatHub> _hubContext;
        public SendRegularMessageToClientsJob(IHubContext<SignalRChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task Execute(IJobExecutionContext context)
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
    }
}
