using Microsoft.AspNetCore.Mvc;
using Quartz;
using System.Reflection;
using TaskTracker_BL;
using TaskTracker_BL.DTOs;
using TaskTracker_BL.Services.QuartzService;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuartsJobsController : ControllerBase
    {
        private IQuartzService _quartzService;
        public QuartsJobsController(
           IQuartzService quartzService)
        {
            _quartzService = quartzService;
        }

        [HttpPost("Start")]
        public async Task StartJob(StartJobRequest startJobRequest)
        {
            await _quartzService.StartJob(startJobRequest);
        }

        [HttpPost("Stop")]
        public async Task StopJob(string jobName)
        {
            await _quartzService.StopJob(jobName);
        }

        [HttpPost("Resume")]
        public async Task ResumeJob(string jobName)
        {
            await _quartzService.ResumeJob(jobName);
        }

        [HttpPost("Delete")]
        public async Task DeleteJob(string jobName)
        {
            await _quartzService.DeleteJob(jobName);
        }
    }
}
