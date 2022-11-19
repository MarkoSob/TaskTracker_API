using AutoMapper;
using TaskTracker_BL.DTOs;
using TaskTracker_DAL.Entities;

namespace TaskTracker_BL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegistrationDto, User>()
                .ForMember(x => x.CreationDate, opt => opt.MapFrom(
                    d=>DateTime.Now.Date));
        }
    }
}
