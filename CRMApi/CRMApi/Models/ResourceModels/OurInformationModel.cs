namespace CRMApi.Models.ResourceModels
{
    /// <summary>
    /// Информация о нас
    /// </summary>
    public class OurInformationModel : OurInformation
    {
        /// <summary>
        /// Фото на карте
        /// </summary>
        public IFormFile Picture { get; set; }
        public new string? GuidPicture { get; set; }
    }
}
