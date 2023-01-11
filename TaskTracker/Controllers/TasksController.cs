using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker_BL.Services.TasksService;
using TaskTracker_BL.DTOs;
using TaskTracker.Core.QueryParameters;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _taskTrackerService;

        public TasksController(ITasksService taskTrackerService)
        {
            _taskTrackerService = taskTrackerService;
        }

        [HttpPost("createtask")]
        public async Task<IActionResult> CreateAsync(CreateUserTaskDto createUserTaskDto)
            => Ok(await _taskTrackerService.CreateAsync(createUserTaskDto));

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            //Response.Headers.Add("X-Total-Count", "100");
            return Ok(await _taskTrackerService.GetAllAsync());
        }
         
        //[Authorize]
        //[HttpGet("usertasks/{email}")]
        //public async Task<IEnumerable<UserTaskDto>> GetAllUserTasksAsync(string email)
        //    => await _taskTrackerService.GetAllUserTasksAsync(email);

        [HttpGet("UserTasks")]
        public async Task<IActionResult> GetAllUserTasksAsync(string email, [FromQuery] QueryParameters<UserTaskDto> parameters)
            => Ok(await _taskTrackerService.GetTasksByTitle(email, parameters));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
            => Ok(await _taskTrackerService.GetByIdAsync(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, CreateUserTaskDto createUserTaskDto)
            => Ok(await _taskTrackerService.UpdateAsync(id, createUserTaskDto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
            => Ok(await _taskTrackerService.DeleteAsync(id));
    }
}