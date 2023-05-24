using System.ComponentModel.DataAnnotations;

namespace CRMWebForWorker.Models.ResourceModels
{
    /// <summary>
    /// Header
    /// </summary>
    public class Header
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
