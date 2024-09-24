using AuthService.Models;

namespace AuthService.Extensions
{
    public static class HttpContextExtension
    {
        public static IdentityUser GetIdentityUser(this HttpContext context)
        {
            var identityUser = FindIdentityUser(context);

            if (identityUser is null)
                throw new InvalidOperationException("Identity user not found");

            return identityUser;
        }

        public static IdentityUser? FindIdentityUser(this HttpContext context)
        {
            return (IdentityUser)context.Items[nameof(IdentityUser)];
        }
    }
}
