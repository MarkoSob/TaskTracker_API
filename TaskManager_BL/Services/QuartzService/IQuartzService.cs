
using TaskTracker_BL.DTOs;

namespace TaskTracker_BL.Services.QuartzService
{
    public interface IQuartzService
    {
        Task DeleteJob(string jobName);
        Task ResumeJob(string jobName);
        Task StartJob(StartJobRequest startJobRequest);
        Task StopJob(string jobName);
    }
}