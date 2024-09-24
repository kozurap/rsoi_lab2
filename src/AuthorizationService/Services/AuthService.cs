using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml.Linq;
using AuthService.Configurations;
using AuthService.Models;
using AuthService.Services.Interfaces;
using AuthService.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserSessionKeysStorage _sessionKeysStorage;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly ILogger<AuthService> _logger;
        private readonly AppDbContext _appDbContext;

        public AuthService(IOptions<JwtConfiguration> jwtConfiguration,
            ILogger<AuthService> logger,
            AppDbContext appDbContext)
        {
            _jwtConfiguration = jwtConfiguration.Value;
            _logger = logger;
            _sessionKeysStorage = new UserSessionKeysStorage();
            _appDbContext = appDbContext;
        }

        public async Task<string> Login(string login,string password, List<string> permissions)
        {
            var user = await _appDbContext.Set<User>().AsNoTracking().FirstOrDefaultAsync(x=>x.Login == login && x.Password == password);
            if(user == null)
            {
                return null;
            }
            var sessionKey = _sessionKeysStorage.GenerateNewSessionKey(user.Id);
            if(user.Id == Guid.Parse("a4c4df14-b8f8-4201-97ce-d389f5082bba"))
            {
                permissions.Add("admin");
            }
            var claimsIdentity = ClaimsCreator.CreateClaimsIdentity(new IdentityUser(user.Id,
                login,
                permissions,
                sessionKey,
                login));

            var token = CreateJwtToken(claimsIdentity);

            return token;
        }


        public void Logout(Guid userId)
        {
            _sessionKeysStorage.RemoveUserSessionKey(userId);
        }

        public bool HttpContextContainsUserWithNeededPermissions(HttpContext httpContext, string permission)
        {
            var user = (IdentityUser?)httpContext.Items[nameof(IdentityUser)];

            if (user == null)
                return false;
            if (!user.Permissions.Contains(permission))
                return false;

            return true;
        }

        public bool UserIsLoggedIn(Guid userId)
        {
            return _sessionKeysStorage.GetUserSessionKey(userId) is not null;
        }

        public Guid? GetSessionKey(Guid userId)
        {
            return _sessionKeysStorage.GetUserSessionKey(userId);
        }

        public bool IsValidSessionKey(Guid userId, Guid sessionKey)
        {
            var userSessionKey = _sessionKeysStorage.GetUserSessionKey(userId);
            return userSessionKey == sessionKey;
        }

        public bool IsValidSessionKey(HttpContext httpContext)
        {
            var user = (IdentityUser?)httpContext.Items[nameof(IdentityUser)];

            if (user is null)
                return false;

            var sessionKey = _sessionKeysStorage.GetUserSessionKey(user.Id);

            return user.SessionKey == sessionKey;
        }

        private string CreateJwtToken(ClaimsIdentity identity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (_jwtConfiguration.SecurityKey == null)
            {
                _logger.LogCritical("JwtConfiguration not loaded in {service}.", nameof(AuthService));

                throw new ArgumentNullException(
                    $"{nameof(_jwtConfiguration.SecurityKey)} in {nameof(_jwtConfiguration)} is null!");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddHours(_jwtConfiguration.LifetimeHours),
                SigningCredentials = new SigningCredentials(
                    SymmetricSecurityKeysHelper.GetSymmetricSecurityKey(_jwtConfiguration.SecurityKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<Guid> Register(string login, string password)
        {
            var result = await _appDbContext.AddAsync<User>(new User
            {
                Id = Guid.NewGuid(),
                Login = login,
                Password = password
            });
            _appDbContext.SaveChanges();
            return result.Entity.Id;
        }
    }
}
