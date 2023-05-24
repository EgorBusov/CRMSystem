using CRMApi.Interfaces;

namespace CRMApi.Services
{
    public class PictureManager : IPictureManager
    {
        /// <summary>
        /// Сохраняет картинку блога
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public async Task<string> SavePicture(IFormFile formFile, string relativePath)
        {
            string extension = Path.GetExtension(formFile.FileName); //получаем формат файла
            if (extension.ToLower() == ".jpeg" || extension.ToLower() == ".jpg")
            {
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string path = Path.Combine(AppContext.BaseDirectory, relativePath, fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);//загружаем картинку в поток
                }
                return fileName;
            }
            else { throw new Exception("Неверный формат"); }
        }
        /// <summary>
        /// Удаляет картинку
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task DeletePicture(string fileName, string relativePath)
        {
            File.Delete(Path.Combine(AppContext.BaseDirectory, relativePath, fileName));
        }
    }
}
