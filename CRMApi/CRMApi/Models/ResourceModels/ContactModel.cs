namespace CRMApi.Models.ResourceModels
{
    public class ContactModel : Contact
    {
        public IFormFile Picture { get; set; }
        public new string? GuidPicture { get; set; }
    }
}
