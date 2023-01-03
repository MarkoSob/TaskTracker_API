using Microsoft.Extensions.Logging;
using Quartz;
using System.Reflection;
using TaskTracker_BL.DTOs;
using TaskTracker_BL.Quartz_Jobs;

namespace TaskTracker_BL.Services.QuartzService
{
    public class QuartzService : IQuartzService
    {
        private ILogger<QuartzService> _logger;
        private ISchedulerFactory _schedulerFactory;
        private static IEnumerable<Type> _jobs;
        public QuartzService(
            ILogger<QuartzService> logger,
            ISchedulerFactory schedulerFactory)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
            if (_jobs == null)
            {
                _jobs = Assembly.GetAssembly(typeof(SendRegularMessageToClientsJob)).ExportedTypes.Where(x => x.GetInterfaces().Any(y => y == typeof(IJob)));
            }
        }
        public async Task StartJob(StartJobRequest startJobRequest)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            JobKey key = new JobKey(startJobRequest.JobTitle);
            var job = _jobs.FirstOrDefault(x => x.Name.EndsWith(startJobRequest.JobTitle, StringComparison.OrdinalIgnoreCase));
            
                await scheduler.ScheduleJob(
               JobBuilder.Create(job)
                    .WithIdentity(startJobRequest.JobTitle)
                    .Build(),
               TriggerBuilder.Create()
                   .WithIdentity(startJobRequest.JobTitle)
                   .StartNow()
                   .WithSimpleSchedule(x => x
                       .WithIntervalInSeconds(startJobRequest.IntervalInSeconds)
                       .WithRepeatCount(startJobRequest.IntervalInSeconds))
                   .Build());

            _logger.LogInformation($"The job {startJobRequest.JobTitle} was launched");
        }

        public async Task StopJob(string jobName)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            JobKey key = new JobKey(jobName);

            await scheduler.PauseJob(key);

            _logger.LogInformation($"The job {jobName} was stopped");
        }

        public async Task ResumeJob(string jobName)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            JobKey key = new JobKey(jobName);

            await scheduler.ResumeJob(key);

            _logger.LogInformation($"The job {jobName} continue to execute");
        }

        public async Task DeleteJob(string jobName)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            JobKey key = new JobKey(jobName);

            await scheduler.DeleteJob(key);

            _logger.LogInformation($"The job {jobName} was removed");
        }

    }
}
