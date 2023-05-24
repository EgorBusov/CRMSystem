using System.ComponentModel.DataAnnotations;

namespace CRMApi.Models.AccountModels
{
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
