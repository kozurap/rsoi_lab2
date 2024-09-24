using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly int _unauthorizedStatusCode;
        private readonly string _permission;

        public AuthorizeAttribute(string permission, int unauthorizedStatusCode = 401)
        {
            _permission = permission;
            _unauthorizedStatusCode = unauthorizedStatusCode;
        }

        public AuthorizeAttribute(int unauthorizedStatusCode = 401)
        {
            _unauthorizedStatusCode = unauthorizedStatusCode;
            _permission = string.Empty;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {

            var httpContext = filterContext.HttpContext;
            var user = (IdentityUser?)httpContext.Items[nameof(IdentityUser)];

            if (user == null)
            {
                filterContext.Result = new JsonResult(new { message = "Unauthorized" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            else if (AuthorizationConflictOccurred(user.Id, user.SessionKey))
            {
                filterContext.Result = new JsonResult(new
                { message = "В ваш аккаунт был выполнен вход с другого устройства." })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            else if (!HasRequiredPermissions(httpContext))
            {
                filterContext.Result = new JsonResult(new { message = "Пользователь не авторизован" })
                {
                    StatusCode = _unauthorizedStatusCode
                };
            }
        }

        private bool AuthorizationConflictOccurred(Guid userId, Guid sessionKey)
        {
            return false;
        }

        private bool HasRequiredPermissions(HttpContext context)
        {
            return string.IsNullOrEmpty(_permission) ||
                   HttpContextContainsUserWithNeededPermissions(context, _permission);
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
    }
}
