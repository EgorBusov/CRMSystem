using System.ComponentModel.DataAnnotations;

namespace CRMApi.Models.AccountModels
{
    /// <summary>
    /// Вход
    /// </summary>
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
