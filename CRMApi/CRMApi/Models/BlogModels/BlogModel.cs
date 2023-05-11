namespace CRMApi.Models.BlogModels
{
    public class BlogModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Picture { get; set; }
    }
}
