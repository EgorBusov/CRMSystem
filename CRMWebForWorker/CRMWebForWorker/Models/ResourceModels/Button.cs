using System.ComponentModel.DataAnnotations;

namespace CRMWebForWorker.Models.ResourceModels
{
    /// <summary>
    /// Информация о кнопке, для визуальной части
    /// </summary>
    public class Button
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
