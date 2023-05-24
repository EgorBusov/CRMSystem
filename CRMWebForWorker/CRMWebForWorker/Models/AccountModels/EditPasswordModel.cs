using System.ComponentModel.DataAnnotations;

namespace CRMWebForWorker.Models.AccountModels
{
    /// <summary>
    /// Изменение пароля
    /// </summary>
    public class EditPasswordModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
