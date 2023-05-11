namespace CRMApi.Models.AccountModels
{
    public class EditPasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public int UserId { get; set; }
    }
}
