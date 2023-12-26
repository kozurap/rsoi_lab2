using AutoMapper;
using Kernel.Extensions;
using PrivilegeService.Dtos;
using PrivilegeService.Entiies;
using PrivilegeService.Filters;

namespace PrivilegeService.Controllers
{
    public class PrivilegesController : RestControllerBase<Privilege, PrivilegeDto, PrivilegeFilter>
    {
        public PrivilegesController(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
        {
        }

        protected override IQueryable<Privilege> AttachFilterToQueryable(IQueryable<Privilege> q, PrivilegeFilter f)
        => q.WhereNext(f.UserName, x => x.Username == f.UserName);

        protected override void MapDtoToEntity(Privilege e, PrivilegeDto dto)
        {
            e.Id = dto.Id;
            e.Status = dto.Status;
            e.PrivilegeHistories = dto.PrivilegeHistories.Select(x => Mapper.Map<PrivilegeHistory>(dto.PrivilegeHistories)).ToList();
            e.Username = dto.Username;
            e.Balance = dto.Balance;
        }
    }
}
