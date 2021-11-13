using Application.Common.Mappings;
using Application.UserManagements.UserProfile.Dtos;
using AutoMapper;
using UserProfileEntity = Domain.Entities.UserManagements.UserProfile;

namespace Application.UserManagements.UserProfile.Mapper
{
    public class UserDetailMapper : MapConfig
    {
        protected override MapperConfiguration CreateConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserProfileEntity, DetailsDto>(MemberList.Source)
                .ForMember(dto => dto.LoginId, conf => conf.MapFrom(s => s.ApplicationUser.UserName))
                .ForMember(dto => dto.UserId, conf => conf.MapFrom(s => s.UserId));
            }
            );
        }
    }
}
