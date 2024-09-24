using System.Collections.Concurrent;

namespace AuthService.Utils
{
    /// <summary>
    /// Хранилище ключей сессий для пользователей.
    /// </summary>
    public class UserSessionKeysStorage
    {
        private readonly ConcurrentDictionary<Guid, Guid> _usersSessionsKeys;

        public UserSessionKeysStorage()
        {
            _usersSessionsKeys = new ConcurrentDictionary<Guid, Guid>();
        }

        /// <summary>
        /// Генерирует новый сессионный ключ.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Новый сессионный ключ для заданного пользователя.</returns>
        public Guid GenerateNewSessionKey(Guid userId)
        {
            var newSessionKey = Guid.NewGuid();

            _usersSessionsKeys[userId] = newSessionKey;

            return newSessionKey;
        }

        /// <summary>
        /// Получить текущий сессионный ключ для пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Сессионный ключ для заданного пользователя.
        /// Null, если пользователь не был найден
        /// </returns>
        public Guid? GetUserSessionKey(Guid userId)
        {
            Guid? result = null;

            if (_usersSessionsKeys.ContainsKey(userId))
                result = _usersSessionsKeys[userId];

            return result;
        }

        /// <summary>
        /// Удалить сессионный ключ пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        public void RemoveUserSessionKey(Guid userId)
        {
            _usersSessionsKeys.TryRemove(userId, out _);
        }
    }
}
