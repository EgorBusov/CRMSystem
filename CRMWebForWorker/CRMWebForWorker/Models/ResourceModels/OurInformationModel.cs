namespace CRMWebForWorker.Models.ResourceModels
{
    /// <summary>
    /// Информация о нас
    /// </summary>
    public class OurInformationModel 
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string? GuidPicture { get; set; }
        /// <summary>
        /// Фото на карте
        /// </summary>
        public IFormFile Picture { get; set; }
    }
}
