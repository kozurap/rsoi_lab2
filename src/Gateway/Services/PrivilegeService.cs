using Gateway.Dtos;
using Kernel.AbstractClasses;
using Kernel.Exceptions;

namespace Gateway.Services
{
    public class PrivilegeService : ClientServiceBase
    {
       

        public PrivilegeService()
        {
            
        }

        protected override string BaseUri => "http://privilegeservice:80";

        public async Task<PrivilegeDto?> GetPrivilegeAsync(string userName) => (await GetAllPrivelegesAsync(1,1,userName))?.Items?.FirstOrDefault();

        public async Task<PaginationModel<PrivilegeDto>?> GetAllPrivelegesAsync(int page, int size, string userName)
        {
            var result = await Client.GetAsync<PaginationModel<PrivilegeDto>>(BuildUri("api/v1/privileges"), null, new Dictionary<string, string>
            {
                {"page", page.ToString() },
                { "size", size.ToString() },
                { "UserName", userName }
            });
            if (result == null)
                throw new NotFoundException();
            return result;
        }

        public async Task<PaginationModel<PrivilegeHistoryDto>?> GetAllPrivilegeHistories(int page, int size) 
            => await Client.GetAsync<PaginationModel<PrivilegeHistoryDto>>(BuildUri("api/v1/privilegeshistory"));


        public async Task<PrivilegeDto?> UpdatePrivilegeAsync(int id, PrivilegeDto dto)
            => await Client.PatchAsync<PrivilegeDto, PrivilegeDto>(BuildUri("api/v1/privileges/" + id), dto);
        public async Task<PrivilegeHistoryDto?> AddPrivilegeHistoryAsync(PrivilegeHistoryDto dto)
            => await Client.PostAsync<PrivilegeHistoryDto, PrivilegeHistoryDto>(BuildUri("api/v1/privilegeshistory/"), dto);

        public async Task<UserPrivilegeDto?> GetUserPrivilegeDto(string userName)
        {
            var privilege = await GetPrivilegeAsync(userName);
            var histories = await GetAllPrivilegeHistories(1, 100);
            var neededHistories = histories.Items.Where(x => x.PrivilegeId == privilege.Id);
            return new UserPrivilegeDto
            {
                Balance = privilege.Balance.Value,
                Status = privilege.Status,
                History = neededHistories.Select(x => new UserPrivilegeHistoryDto
                {
                    BalanceDiff = x.BalanceDiff,
                    Datetime = x.Datetime,
                    OperationType = x.OperationType,
                    TicketUid = x.TicketUid
                }).ToList()
            };
        }
    }
}
