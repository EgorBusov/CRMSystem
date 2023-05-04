using CRMApi.Models;
using CRMApi.Models.ModelsApi;

namespace CRMApi.Interfaces
{
    public interface IProjectData
    {
        /// <summary>
        /// Добавление проекта
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task AddProject(IFormFile formFile, string title, string description);
        /// <summary>
        /// Удаление проекта
        /// </summary>
        /// <param name="idProject"></param>
        /// <returns></returns>
        Task DeleteProject(int idProject);
        /// <summary>
        /// Получение всех Project в виде ProjectModel
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProjectModel>> GetProjectModels();
        /// <summary>
        /// Изменение проекта
        /// </summary>
        /// <param name="p"></param>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task EditProject(Project p, IFormFile formFile);
        /// <summary>
        /// Поиск Project по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProjectModel> GetProjectById(int id);
    }
}
