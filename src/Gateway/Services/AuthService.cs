using Gateway.Dtos;
using Gateway.Models;
using Kernel.AbstractClasses;

namespace Gateway.Services
{
    public class AuthService : ClientServiceBase
    {
        protected override string BaseUri => "http://authorizationservice:8040";//8040

        public async Task<JwtToken> Login(AuthDto dto) =>
            await Client.PostAsync<JwtToken, AuthDto>(BuildUri("api/v1/user/login"), dto);

        public async Task<Guid> Register(AuthDto dto) =>
            await Client.PostAsync<Guid, AuthDto>(BuildUri("api/v1/user/register"), dto);
    }
}
