using System.ComponentModel.DataAnnotations;

namespace CRMWebForWorker.Models.ServiceModels
{
    /// <summary>
    /// Услуги
    /// </summary>
    public class Service
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
