namespace CRMApi.Models
{
    /// <summary>
    /// Запись в блоге
    /// </summary>
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string GuidPicture { get; set; }
    }
}
