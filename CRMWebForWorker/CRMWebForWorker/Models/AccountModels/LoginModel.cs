using System.ComponentModel.DataAnnotations;

namespace CRMWebForWorker.Models.AccountModels
{
    /// <summary>
    /// Залогинивание
    /// </summary>
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
