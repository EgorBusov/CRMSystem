namespace CRMApi.Interfaces
{
    public interface IPictureManager
    {
        /// <summary>
        /// Сохраняет картинку
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task<string> SavePicture(IFormFile formFile);
        /// <summary>
        /// Удаляет картинку
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task DeletePicture(string fileName);
    }
}
