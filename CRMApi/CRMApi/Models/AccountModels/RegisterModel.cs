using System.ComponentModel.DataAnnotations;

namespace CRMApi.Models.AccountModels
{
    public class RegisterModel
    {
        [Required]
        public string UserName {  get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

    }
}
