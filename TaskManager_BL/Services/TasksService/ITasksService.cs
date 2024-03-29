﻿using TaskTracker.Core.QueryParameters;
using TaskTracker_BL.DTOs;

namespace TaskTracker_BL.Services.TasksService
{
    public interface ITasksService
    {
        Task<UserTaskDto> CreateAsync(CreateUserTaskDto createUserTaskDto);
        Task<UserTaskDto> DeleteAsync(Guid? id);
        Task<IEnumerable<UserTaskDto>> GetAllAsync();
        Task<IEnumerable<UserTaskDto>> GetAllUserTasksAsync(string email);
        Task<IEnumerable<UserTaskDto>> GetTasksByTitle(string email, QueryParameters<UserTaskDto> parameters);
        Task<UserTaskDto> GetByIdAsync(Guid? id);
        Task<UserTaskDto> UpdateAsync(Guid? id, CreateUserTaskDto createUserTaskDto);
    }
}