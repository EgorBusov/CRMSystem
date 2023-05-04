using CRMApi.Models;

namespace CRMApi.Interfaces
{
    /// <summary>
    /// Взаимодействие с бд касаемо пользователя
    /// </summary>
    public interface IAccountData
    {
        /// <summary>
        /// Поиск пользователя по логину и паролю
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        User GetUserByLogPass(User user);
        /// <summary>
        /// Возврат всех пользователей
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetUsers();
        /// <summary>
        /// Добавление пользователя
        /// </summary>
        /// <param name="user"></param>
        void AddUser(User user);
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="user"></param>
        void RemoveUser(int userId);
        /// <summary>
        /// Проверка на актуальность Username
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool CheckUserName(User user);
        /// <summary>
        /// Поиск пользователя по логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        User GetUserByLogin(string login);
    }
}
