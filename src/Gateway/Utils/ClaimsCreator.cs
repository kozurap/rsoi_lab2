using System.Security.Claims;
using Gateway.Attributes;

namespace AuthService.Utils
{
    public class ClaimsCreator
    {
        /// <summary>
        /// Создает сущность ClaimsIdentity по <paramref name="identityUser"/>.
        /// </summary>
        /// <param name="identityUser">Модель пользователя с авторизационными данными.</param>
        /// <returns>ClaimsIdentity для авторизационного пользователя.</returns>
        public static ClaimsIdentity CreateClaimsIdentity(IdentityUser identityUser)
        {
            var claims = new List<Claim>
            {
                new Claim(nameof(IdentityUser.Login), identityUser.Login),
                new Claim(nameof(IdentityUser.Permissions), PermissionsToString(identityUser.Permissions)),
                new Claim(nameof(IdentityUser.Id), identityUser.Id.ToString()),
                new Claim(nameof(IdentityUser.SessionKey), identityUser.SessionKey.ToString()),
                new Claim(nameof(IdentityUser.Name), identityUser.Name)
            };

            return new ClaimsIdentity(claims,
                authenticationType: "Token",
                nameType: nameof(IdentityUser.Login),
                roleType: nameof(IdentityUser.Permissions));
        }

        private static string PermissionsToString(List<string> permissions)
        {
            return permissions.Aggregate("",
                (total, next) => total += next + ",",
                (total) => total.EndsWith(',') ? total[..^1] : total);
        }

        private static List<string> PermissionsToList(string permissions)
        {
            return permissions
                .Split(",")
                .ToList();
        }

        /// <summary>
        /// Создает сущность IdentityUser по коллекции claims.
        /// </summary>
        /// <param name="claims">Коллекция клеймов для пользователя.</param>
        /// <returns>IdentityUser на основе коллекции <paramref name="claims"/>.</returns>
        public static IdentityUser? ParseClaimsToIdentityUser(IEnumerable<Claim> claims)
        {
            try
            {
                var userId = ReturnIdClaimAsGuid(claims);
                var name = claims.First(c => c.Type == nameof(IdentityUser.Name)).Value;
                var login = claims.First(c => c.Type == nameof(IdentityUser.Login)).Value;
                var permissions = claims.First(c => c.Type == nameof(IdentityUser.Permissions)).Value;
                var sessionKey = Guid.Parse(claims
                    .First(c => c.Type == nameof(IdentityUser.SessionKey)).Value);

                return new IdentityUser(userId!.Value, login, PermissionsToList(permissions), sessionKey, name);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Среди клеймов найти тот, который представляет собой Id пользователя и распарсить его в Guid.
        /// </summary>
        /// <param name="claims">Список клеймов с авторизационными данными.</param>
        /// <returns>Id пользователя как Guid. Null, если среди клеймов не будет Id или если он не парсится к Guid.</returns>
        public static Guid? ReturnIdClaimAsGuid(IEnumerable<Claim> claims)
        {
            try
            {
                return Guid.Parse(claims.First(c => c.Type == nameof(IdentityUser.Id)).Value);
            }
            catch
            {
                return null;
            }
        }
    }
}
