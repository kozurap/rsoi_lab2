using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Utils
{
    public class SymmetricSecurityKeysHelper
    {
        /// <summary>
        /// Генерирует симметричный ключ по <paramref name="rawString"/>
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string rawString)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(rawString));
        }
    }
}
