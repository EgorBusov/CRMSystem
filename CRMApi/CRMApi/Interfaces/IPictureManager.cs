namespace CRMApi.Interfaces
{
    public interface IPictureManager
    {
        /// <summary>
        /// Сохраняет картинку
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task<string> SavePicture(IFormFile formFile, string relativePath);
        /// <summary>
        /// Удаляет картинку
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task DeletePicture(string fileName, string relativePath);
    }
}
