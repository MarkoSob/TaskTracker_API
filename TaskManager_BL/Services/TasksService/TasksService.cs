using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskTracker.Core.Extensions;
using TaskTracker_BL.DTOs;
using TaskTracker.Core.Exceptions.DomainExceptions;
using TaskTracker_DAL;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.GenericRepository;
using TaskTracker.Core.Exceptions.DataAccessExceptions;
using TaskTracker.Core.QueryParameters;

namespace TaskTracker_BL.Services.TasksService
{
    public class TasksService : ITasksService
    {
        private readonly IGenericRepository<UserTask> _genericUserTaskRepository;
        private readonly IGenericRepository<User> _genericUserRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TasksService> _logger;

        public TasksService(
            IGenericRepository<UserTask> genericUserTaskRepository,
            IGenericRepository<User> genericUserRepository,
            ILogger<TasksService> logger,
            IMapper mapper)
        {
            _genericUserRepository = genericUserRepository;
            _genericUserTaskRepository = genericUserTaskRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserTaskDto> CreateAsync(CreateUserTaskDto createUserTaskDto)
        {
            User user = _genericUserRepository.GetByPredicate(x => x.Email == createUserTaskDto.UserEmail).FirstOrDefault();

            if (user is null)
            {
                _logger.LogAndThrowException(new ObjectNotFoundException(typeof(User)));
            }

            UserTask newUserTask = _mapper.Map<UserTask>(createUserTaskDto);
            newUserTask.User = user;
            newUserTask.Status = UserTaskStatus.New;

            return _mapper.Map<UserTaskDto>(await _genericUserTaskRepository.CreateAsync(newUserTask));
        }

        public async Task<IEnumerable<UserTaskDto>> GetAllAsync() =>
            _mapper.Map<IEnumerable<UserTaskDto>>(await _genericUserTaskRepository.GetAllAsync());


        public async Task<IEnumerable<UserTaskDto>> GetAllUserTasksAsync(string email) =>
            _mapper.Map<IEnumerable<UserTaskDto>>(_genericUserTaskRepository.GetByPredicate(x => x.User.Email == email));

        public async Task<IEnumerable<UserTaskDto>> GetTasksByTitle(string email, QueryParameters<UserTaskDto> parameters)
        {
            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                return _mapper.Map<IEnumerable<UserTaskDto>>(_genericUserTaskRepository
                    .GetByPredicate(x => x.Title.Contains(parameters.SearchTerm) && x.User.Email == email).Sort(parameters.OrderBy));   
            }

            return _mapper.Map<IEnumerable<UserTaskDto>>(_genericUserTaskRepository.GetByPredicate(x => x.User.Email == email).Sort(parameters.OrderBy));
        }
            
        public async Task<UserTaskDto> GetByIdAsync(Guid? id)
        {
            if (id is null)
            {
                _logger.LogAndThrowException(new NullIdException());
            }

            return _mapper.Map<UserTaskDto>(await _genericUserTaskRepository.GetByIdAsync(id));
        }

        public async Task<UserTaskDto> UpdateAsync(Guid? id, CreateUserTaskDto createUserTaskDto)
        {
            if (id is null)
            {
                _logger.LogAndThrowException(new NullIdException());
            }

            User user = _genericUserRepository.GetByPredicate(x => x.Email == createUserTaskDto.UserEmail).FirstOrDefault();

            if (user is null)
            {
                _logger.LogAndThrowException(new ObjectNotFoundException(typeof(User)));
            }

            UserTask userTask = _mapper.Map<UserTask>(createUserTaskDto);

            userTask.Id = (Guid)id;
            userTask.User = user;

            return _mapper.Map<UserTaskDto>(await _genericUserTaskRepository.UpdateAsync(userTask));
        }

        public async Task<UserTaskDto> DeleteAsync(Guid? id)
        {
            if (id is null)
            {
                _logger.LogAndThrowException(new NullIdException());
            }

            return _mapper.Map<UserTaskDto>(await _genericUserTaskRepository.DeleteAsync(id));
        }
    }         
}