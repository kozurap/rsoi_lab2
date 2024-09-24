using AutoMapper;
using Kernel.Extensions;
using PrivilegeService.Dtos;
using PrivilegeService.Entiies;
using PrivilegeService.Filters;

namespace PrivilegeService.Controllers
{
    public class PrivilegesHistoryController : RestControllerBase<PrivilegeHistory, PrivilegeHistoryDto, PrivilegeHistoryFilter>
    {
        public PrivilegesHistoryController(IMapper mapper, AppDbContext dbContext, LogsProducer producer) : base(mapper, dbContext, producer)
        {
        }

        protected override void MapDtoToEntity(PrivilegeHistory privilegeHistory, PrivilegeHistoryDto privilegeHistoryDto)
        {
            privilegeHistory.Id = privilegeHistoryDto.Id;
            privilegeHistory.BalanceDiff = privilegeHistoryDto.BalanceDiff;
            privilegeHistory.OperationType = privilegeHistoryDto.OperationType;
            privilegeHistory.TicketUid = privilegeHistoryDto.TicketUid;
            privilegeHistory.Datetime = privilegeHistoryDto.Datetime;
            privilegeHistory.Privilege = Mapper.Map<Privilege>(privilegeHistoryDto.Privilege);
            privilegeHistory.PrivilegeId = privilegeHistoryDto.PrivilegeId;
        }
    }
}
