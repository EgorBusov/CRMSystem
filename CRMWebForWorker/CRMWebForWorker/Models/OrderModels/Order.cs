using System.ComponentModel.DataAnnotations;

namespace CRMWebForWorker.Models.OrderModels
{
    /// <summary>
    /// Заявка
    /// </summary>
    public class Order
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SurName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Text { get; set; }
        public string? DateCreate { get; set; }
        public string? Status { get; set; }
    }
}
