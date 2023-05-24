namespace CRMApi.Models.BlogModels
{
    public class BlogModel : Blog
    {
        public IFormFile Picture { get; set; }
        public new string? GuidPicture { get; set; }
    }
}
