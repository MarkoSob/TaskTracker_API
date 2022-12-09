using Quartz;
using TaskTracker.Core;
using TaskTracker_DAL;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.GenericRepository;

namespace TaskTracker_BL.Quartz_Jobs
{
    public class CheckIfTasksOverdue: IJob
    {
        private readonly IGenericRepository<UserTask> _taskRepository;

        public CheckIfTasksOverdue(IGenericRepository<UserTask> taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            List<UserTask> tasks = (await _taskRepository.GetAllAsync()).ToList();
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].EndDate.CompareTo(DateTime.Now) < 1)
                {
                    tasks[i].Status = UserTaskStatus.Overdue;
                    await _taskRepository.UpdateAsync(tasks[i]);
                }
            }
        }

    }
}
