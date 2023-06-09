﻿using CRMApi.Models;
using CRMApi.Models.BlogModels;

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
        Task AddBlog(BlogModel model);
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
        Task<IEnumerable<BlogPath>> GetBlogs();
        /// <summary>
        /// Изменение Blog
        /// </summary>
        /// <param name="b"></param>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task EditBlog(BlogModel model);
        /// <summary>
        /// Поиск Blog по Id. Возврат BlogModel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BlogPath> GetBlogById(int id);
    }
}
