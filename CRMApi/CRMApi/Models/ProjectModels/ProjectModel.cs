namespace CRMApi.Models.ProjectModels
{
    public class ProjectModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Picture { get; set; }
    }
}
