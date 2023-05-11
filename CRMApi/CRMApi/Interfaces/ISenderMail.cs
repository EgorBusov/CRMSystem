using CRMApi.Models.AccountModels;

namespace CRMApi.Interfaces
{
    /// <summary>
    /// Отправка писем на почту
    /// </summary>
    public interface ISenderMail
    {
        /// <summary>
        /// Отправка письма
        /// </summary>
        /// <param name="user"></param>
        /// <param name="themeMail"></param>
        /// <param name="message"></param>
        void SendMail(string email, string themeMail, string message);
    }
}
