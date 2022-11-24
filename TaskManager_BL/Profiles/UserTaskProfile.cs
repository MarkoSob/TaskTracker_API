using AutoMapper;
using TaskTracker_BL.DTOs;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.GenericRepository;

namespace TaskTracker_BL.Profiles
{
    public class UserTaskProfile : Profile
    {
        private IGenericRepository<User> _genericRepository;

        public UserTaskProfile(IGenericRepository<User> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public UserTaskProfile()
        {
            CreateMap<CreateUserTaskDto, UserTask>()
                .ForMember(x => x.Status, opt => opt.MapFrom(t=>Enum.Parse<TaskTracker_DAL.UserTaskStatus>(t.Status)));

            CreateMap<UserTask, UserTaskDto>()
                .ForMember(x => x.Status, opt => opt.MapFrom(t => t.Status.ToString()));
            
        }
    }
}
