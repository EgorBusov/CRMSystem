using System.ComponentModel.DataAnnotations;

namespace CRMWeb.Models.AccountModels
{
    public class EditPasswordModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
