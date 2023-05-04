using CRMApi.Models;
using CRMApi.Models.ModelsApi;

namespace CRMApi.Interfaces
{
    /// <summary>
    /// Взаимодействие с бд касаемо блога
    /// </summary>
    public interface IBlogData
    {
        /// <summary>
        /// Сохраняет Blog
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task AddBlog(IFormFile formFile, string title, string description);
        /// <summary>
        /// Удаляет Blog
        /// </summary>
        /// <param name="idBlog"></param>
        /// <returns></returns>
        Task DeleteBlog(int idBlog);
        /// <summary>
        /// Получение всех Blogs в виде BlogModel
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BlogModel>> GetBlogModels();
        /// <summary>
        /// Изменение Blog
        /// </summary>
        /// <param name="b"></param>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task EditBlog(Blog b, IFormFile formFile);
        /// <summary>
        /// Поиск Blog по Id. Возврат BlogModel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BlogModel> GetBlogById(int id);
    }
}
