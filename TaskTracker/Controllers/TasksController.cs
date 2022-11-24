using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker_BL.Services.TasksService;
using TaskTracker_BL.DTOs;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
  
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _taskTrackerService;

        public TasksController(ITasksService taskTrackerService)
        {
            _taskTrackerService = taskTrackerService;
        }

        [HttpPost("createtask")]
        public async Task<UserTaskDto> CreateAsync(CreateUserTaskDto createUserTaskDto)
            => await _taskTrackerService.CreateAsync(createUserTaskDto);

        //[Authorize]
        [HttpGet]
        public async Task<IEnumerable<UserTaskDto>> GetAllAsync()
            => await _taskTrackerService.GetAllAsync();

        [HttpGet("usertasks/{userId}")]
        public async Task<IEnumerable<UserTaskDto>> GetAllUSerTasksAsync(Guid userId)
            => await _taskTrackerService.GetAllUserTasksAsync(userId);

        [HttpGet("{id}")]
        public async Task<UserTaskDto> GetByIdAsync(Guid id)
            => await _taskTrackerService.GetByIdAsync(id);

        [HttpPut("{id}")]
        public async Task<UserTaskDto> UpdateAsync(Guid id, CreateUserTaskDto createUserTaskDto)
            => await _taskTrackerService.UpdateAsync(id, createUserTaskDto);

        [HttpDelete("{id}")]
        public async Task<UserTaskDto> DeleteAsync(Guid id)
            => await _taskTrackerService.DeleteAsync(id);
    }
}