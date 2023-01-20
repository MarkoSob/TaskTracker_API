using AutoMapper;
using System.Globalization;
using TaskTracker_BL.DTOs;
using TaskTracker_DAL.Entities;

namespace TaskTracker_BL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegistrationDto, User>();

            CreateMap<User, UserForAdminViewDto>()
                .ForMember(x => x.CreationDate, opt => opt.MapFrom(
                    d => d.CreationDate.ToString("MM.dd.yyyy HH:mm")));

            CreateMap<User, UserProfileDataDto>()
               .ForMember(x => x.CreationDate, opt => opt.MapFrom(
                   d => d.CreationDate.ToString("MM.dd.yyyy HH:mm")));

            CreateMap<UserProfileDataDto, User>()
              .ForMember(x => x.CreationDate, opt => opt.MapFrom(
                  d => DateTime.ParseExact(d.CreationDate!, "MM.dd.yyyy HH:mm", CultureInfo.InvariantCulture)));
        }
    }
    
}
