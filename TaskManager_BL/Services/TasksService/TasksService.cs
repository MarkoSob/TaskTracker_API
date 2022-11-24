using AutoMapper;
using TaskTracker_BL.DTOs;
using TaskTracker_DAL;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.GenericRepository;

namespace TaskTracker_BL.Services.TasksService
{
    public class TasksService : ITasksService
    {
        private readonly IGenericRepository<UserTask> _genericUserTaskRepository;
        private readonly IGenericRepository<User> _genericUserRepository;
        private readonly IMapper _mapper;

        public TasksService(
            IGenericRepository<UserTask> genericUserTaskRepository, 
            IGenericRepository<User> genericUserRepository,
            IMapper mapper)
        {
            _genericUserRepository = genericUserRepository;
            _genericUserTaskRepository = genericUserTaskRepository;
            _mapper = mapper;
        }

        public async Task<UserTaskDto> CreateAsync(CreateUserTaskDto createUserTaskDto)
        {
            User user =  _genericUserRepository.GetByPredicate(x=>x.Email == createUserTaskDto.UserEmail).FirstOrDefault();
            UserTask newUserTask = _mapper.Map<UserTask>(createUserTaskDto);
            newUserTask.User = user;
            newUserTask.Status = UserTaskStatus.New;
            return _mapper.Map<UserTaskDto>(await _genericUserTaskRepository.CreateAsync(newUserTask));
        }

        public async Task<IEnumerable<UserTaskDto>> GetAllAsync() =>
            _mapper.Map<IEnumerable<UserTaskDto>>(await _genericUserTaskRepository.GetAllAsync());
        

        public async Task<IEnumerable<UserTaskDto>> GetAllUserTasksAsync(Guid id) =>
            _mapper.Map<IEnumerable<UserTaskDto>>(_genericUserTaskRepository.GetByPredicate(x => x.UserId == id));
        

        public async Task<UserTaskDto> GetByIdAsync(Guid id)
            => _mapper.Map<UserTaskDto>(await _genericUserTaskRepository.GetByIdAsync(id));

        public async Task<UserTaskDto> UpdateAsync(Guid id, CreateUserTaskDto createUserTaskDto)
        {
            User user = _genericUserRepository.GetByPredicate(x => x.Email == createUserTaskDto.UserEmail).FirstOrDefault();
            UserTask userTask = _mapper.Map<UserTask>(createUserTaskDto);
            userTask.Id = id;
            userTask.User = user;
            return _mapper.Map<UserTaskDto>(await _genericUserTaskRepository.UpdateAsync(userTask));
        }

        public async Task<UserTaskDto> DeleteAsync(Guid id)
            => _mapper.Map<UserTaskDto>(await _genericUserTaskRepository.DeleteAsync(id));
    }
}