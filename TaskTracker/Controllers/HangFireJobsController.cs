using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskTracker_BL;
using TaskTracker_BL.SignalR;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HangFireJobsController : ControllerBase
    {
        private ILogger<HangFireJobsController> _logger;
        IMessageSenderService _messageSenderService;
        RecurringJobManager recurringJobManager;

        public HangFireJobsController(ILogger<HangFireJobsController> logger, IMessageSenderService messageSenderService)
        {
            _logger = logger;
            _messageSenderService = messageSenderService;
            recurringJobManager = new RecurringJobManager();
        }

        [HttpPost("Start")]
        public async Task StartJob(string jobId)
        {
            var job = new Job(typeof(MessageSenderService), typeof(MessageSenderService).GetMethod(jobId));
            recurringJobManager.AddOrUpdate(jobId, job, "1-59 * * * *");
            recurringJobManager.TriggerJob(jobId);
        }

        [HttpPost("Stop")]
        public async Task StopJob(string jobId)
        {
            recurringJobManager.RemoveIfExists(jobId);
        }
    }
}
