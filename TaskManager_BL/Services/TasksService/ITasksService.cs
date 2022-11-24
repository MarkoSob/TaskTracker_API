using TaskTracker_BL.DTOs;

namespace TaskTracker_BL.Services.TasksService
{
    public interface ITasksService
    {
        Task<UserTaskDto> CreateAsync(CreateUserTaskDto createUserTaskDto);
        Task<UserTaskDto> DeleteAsync(Guid id);
        Task<IEnumerable<UserTaskDto>> GetAllAsync();
        Task<IEnumerable<UserTaskDto>> GetAllUserTasksAsync(Guid id);
        Task<UserTaskDto> GetByIdAsync(Guid id);
        Task<UserTaskDto> UpdateAsync(Guid id, CreateUserTaskDto createUserTaskDto);
    }
}