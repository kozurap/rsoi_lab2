namespace AuthService.Services.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Авторизировать пользователя.
        /// </summary>
        /// <param name="id">Id пользователя.</param>
        /// <param name="login">Login пользователя.</param>
        /// <param name="permissions">Permissions пользователя.</param>
        /// <param name="name">Имя пользователя.</param>
        /// <returns>Jwt токен.</returns>
        Task<string> Login(string login, string password, List<string> permissions);

        Task<Guid> Register(string login, string password);

        /// <summary>
        /// Разлогинить пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя, которого надо разлогинить.</param>
        void Logout(Guid userId);

        /// <summary>
        /// Проверка сессионного ключа пользователя на валидность.
        /// </summary>
        /// <param name="userId">Id пользователя, сессионный ключ которого надо проверить.</param>
        /// <param name="sessionKey">Сессионный ключ, который надо проверить.</param>
        /// <returns>True если сессионный ключ пользователя совпадает с сессионным ключом в хранилище.</returns>
        bool IsValidSessionKey(Guid userId, Guid sessionKey);

        /// <summary>
        /// Проверка сессионного ключа пользователя на валидность.
        /// </summary>
        /// <param name="httpContext">Контекст с IdentityUser.</param>
        /// <returns>True если сессионный ключ пользователя совпадает с сессионным ключом в хранилище.</returns>
        bool IsValidSessionKey(HttpContext httpContext);

        /// <summary>
        /// Проверяет есть ли в контексте запроса модель IdentityUser с необходимыми правами.
        /// </summary>
        /// <param name="httpContext">HttpContext запроса.</param>
        /// <param name="permission">Необходимые права для запроса.</param>
        /// <returns>True если в контексте содержится модель IdentityUser с необходимыми правами.</returns>
        bool HttpContextContainsUserWithNeededPermissions(HttpContext httpContext, string permission);

        /// <summary>
        /// Авторизован ли пользователь с указанным <see cref="userId"/>.
        /// </summary>
        /// <param name="userId">Id проверяемого пользователя.</param>
        /// <returns>True - авториизован. False - не авторизован.</returns>
        bool UserIsLoggedIn(Guid userId);

        /// <summary>
        /// Возвращает сессионный ключ для данного пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя для которого надо вернуть ключ.</param>
        /// <returns>Сессионный ключ, null если пользователь не авторизован.</returns>
        Guid? GetSessionKey(Guid userId);
    }
}
