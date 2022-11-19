using TaskTracker_BL.DTOs;

namespace TaskTracker_BL.Services.TaskTrackerService
{
    public interface ITaskTrackerService
    {
        Task<UserTaskDto> CreateAsync(CreateUserTaskDto createUserTaskDto);
        Task<UserTaskDto> DeleteAsync(Guid id);
        Task<IEnumerable<UserTaskDto>> GetAllAsync();
        Task<UserTaskDto> GetByIdAsync(Guid id);
        Task<UserTaskDto> UpdateAsync(Guid id, CreateUserTaskDto createUserTaskDto);
    }
}