namespace CRMApi.Models.ProjectModels
{
    public class ProjectModel : Project
    {
        public IFormFile Picture { get; set; }
        public new string? GuidPicture { get; set; }
    }
}
