using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthService.Utils;
using Gateway.Attributes;
using Gateway.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next,
            IOptions<JwtConfiguration> jwtConfiguration,
            ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _jwtConfiguration = jwtConfiguration.Value;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (!string.IsNullOrWhiteSpace(token))
                    AttachUserToContext(context, token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in middleware {middleware}.", nameof(JwtMiddleware));
            }
            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                if (tokenHandler.CanReadToken(token))
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            SymmetricSecurityKeysHelper.GetSymmetricSecurityKey(_jwtConfiguration.SecurityKey!),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;

                    var identityUser = ClaimsCreator.ParseClaimsToIdentityUser(jwtToken.Claims);

                    if (identityUser != null)
                    {
                        context.Items[nameof(IdentityUser)] = identityUser;

                        var claimsIdentity = ClaimsCreator.CreateClaimsIdentity(identityUser);
                        context.User = new ClaimsPrincipal(claimsIdentity);
                    }
                }
            }
            catch (SecurityTokenException e)
            {
                _logger.LogInformation(e, "Authorization Jwt token: {token} is invalid.", token);
            }
        }
    }
}
