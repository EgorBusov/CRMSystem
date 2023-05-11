using CRMApi.Models.AccountModels;

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
        User GetUserByLogPass(LoginModel model);
        /// <summary>
        /// Возврат всех пользователей
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetUsers();
        /// <summary>
        /// Добавление пользователя
        /// </summary>
        /// <param name="user"></param>
        string AddUser(RegisterModel model);
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
        public bool CheckUserName(string userName);
        /// <summary>
        /// Поиск пользователя по логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        User GetUserByLogin(string login);
        /// <summary>
        /// Изменение пароля
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="userId"></param>
        void EditPassword(EditPasswordModel edit);
        /// <summary>
        /// Поиск User по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetUserById(int id);
    }
}
