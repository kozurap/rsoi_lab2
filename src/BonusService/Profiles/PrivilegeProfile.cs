using AutoMapper;
using PrivilegeService.Dtos;
using PrivilegeService.Entiies;

namespace PrivilegeService.Profiles
{
    public class PrivilegeProfile : Profile
    {
        public PrivilegeProfile()
        {
            CreateMap<Privilege, PrivilegeDto>();
            CreateMap<PrivilegeDto, Privilege>();
            CreateMap<PrivilegeHistory, PrivilegeHistoryDto>();
            CreateMap<PrivilegeHistoryDto, PrivilegeHistory>();
        }
    }
}
