using CRMApi.Models.AccountModels;

namespace CRMApi.Interfaces
{
    public interface IJWT
    {
        /// <summary>
        /// Хэширование пароля
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string HashPassword(string password);
        /// <summary>
        /// Генерация jwt-токена
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        string GenerateJWT(User userInfo);
    }
}
