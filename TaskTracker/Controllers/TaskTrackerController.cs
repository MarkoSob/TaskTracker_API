using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker_BL.Services.TaskTrackerService;
using TaskTracker_BL.DTOs;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
  
    public class TaskTrackerController : ControllerBase
    {
        private readonly ITaskTrackerService _taskTrackerService;

        public TaskTrackerController(ITaskTrackerService taskTrackerService)
        {
            _taskTrackerService = taskTrackerService;
        }

        [HttpPost]
        public async Task<UserTaskDto> CreateAsync(CreateUserTaskDto createUserTaskDto)
            => await _taskTrackerService.CreateAsync(createUserTaskDto);

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<UserTaskDto>> GetAllAsync()
            => await _taskTrackerService.GetAllAsync();

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