using AutoMapper;
using System.Globalization;
using TaskTracker_BL.DTOs;
using TaskTracker_DAL.Entities;
using TaskTracker_DAL.GenericRepository;
using TaskTracker_DAL;

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
                .ForMember(x => x.Status, opt => opt.MapFrom(t => Enum.Parse<UserTaskStatus>(t.Status!)))
                .ForMember(x => x.Priority, opt => opt.MapFrom(t => Enum.Parse<UserTaskPriority>(t.Priority!)))
                .ForMember(x => x.StartDate, opt => opt.MapFrom(t => DateTime.ParseExact(t.StartDate!, "MM.dd.yyyy HH:mm", CultureInfo.InvariantCulture)))
                .ForMember(x => x.EndDate, opt => opt.MapFrom(t => DateTime.ParseExact(t.EndDate!, "MM.dd.yyyy HH:mm", CultureInfo.InvariantCulture)));

            CreateMap<UserTask, UserTaskDto>()
                .ForMember(x => x.Status, opt => opt.MapFrom(t => t.Status.ToString()))
                .ForMember(x => x.Priority, opt => opt.MapFrom(t => t.Priority.ToString()))
                .ForMember(x => x.StartDate, opt => opt.MapFrom(t => t.StartDate.ToString("MM.dd.yyyy HH:mm")))
                .ForMember(x => x.EndDate, opt => opt.MapFrom(t => t.EndDate.ToString("MM.dd.yyyy HH:mm")))
                .ForMember(x => x.CreationDate, opt => opt.MapFrom(t => t.CreationDate.ToString("MM.dd.yyyy HH:mm")));


        }
    }
}
