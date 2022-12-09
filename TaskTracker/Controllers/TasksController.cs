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

        [HttpPost]
        public async Task<UserTaskDto> CreateAsync(CreateUserTaskDto createUserTaskDto)
            => await _taskTrackerService.CreateAsync(createUserTaskDto);

        [HttpGet]
        public async Task<IEnumerable<UserTaskDto>> GetAllAsync()
        {
            //Response.Headers.Add("X-Total-Count", "100");
            return await _taskTrackerService.GetAllAsync();
        }
         
        [Authorize]
        [HttpGet("usertasks/{email}")]
        public async Task<IEnumerable<UserTaskDto>> GetAllUSerTasksAsync(string email)
            => await _taskTrackerService.GetAllUserTasksAsync(email);

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