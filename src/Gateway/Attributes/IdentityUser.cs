namespace Gateway.Attributes
{
    public class IdentityUser
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login { get; }

        /// <summary>
        /// Права пользователя.
        /// </summary>
        public List<string> Permissions { get; }

        /// <summary>
        /// Сессионный ключ пользователя. Представляет собой Id текущей авторизационной сессии. 
        /// </summary>
        public Guid SessionKey { get; }

        public IdentityUser(Guid id,
            string login,
            List<string> permissions,
            Guid sessionKey,
            string name)
        {
            Id = id;
            Login = login;
            Permissions = permissions;
            SessionKey = sessionKey;
            Name = name;
        }
    }
}
