using System.ComponentModel.DataAnnotations;

namespace CRMWebForWorker.Models.BlogModels
{
    /// <summary>
    /// Запись в блоге
    /// </summary>
    public class BlogModel
    {
        public int? Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Picture { get; set; }
    }
}
