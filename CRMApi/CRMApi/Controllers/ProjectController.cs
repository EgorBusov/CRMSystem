using CRMApi.Interfaces;
using CRMApi.Models.ProjectModels;
using CRMApi.Services.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectData _projectData;
        public ProjectController(IProjectData projectData)
        {
            _projectData = projectData;
        }
        /// <summary>
        /// Добавление проекта
        /// </summary>
        /// <param name="projectModel"></param>
        /// <returns></returns>
        [Route("AddProject")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProject([FromForm] ProjectModel projectModel)
        {
            try
            {
                if (ModelState.IsValid) { await _projectData.AddProject(projectModel); return Ok(); }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Удаление проекта
        /// </summary>
        /// <param name="idBlog"></param>
        /// <returns></returns>
        [Route("DeleteProject/{id}")]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                await _projectData.DeleteProject(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Получение всех проектов
        /// </summary>
        /// <returns></returns>
        [Route("GetProjectModels")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<ProjectModel>> GetProjectModels()
        {
            return await _projectData.GetProjectModels();
        }
        /// <summary>
        /// Изменение проекта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("EditProject")]
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProject([FromForm] ProjectModel model)
        {
            try
            {
                if (ModelState.IsValid) { await _projectData.EditProject(model); return Ok(); }
                return BadRequest("Поля заполнены неверно");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Получение блога по Id
        /// </summary>
        /// <param name="idBlog"></param>
        /// <returns></returns>
        [Route("GetProjectById/{id}")]
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ProjectModel> GetProjectById(int id)
        {
            try
            {
                return await _projectData.GetProjectById(id);
            }
            catch
            {
                return new ProjectModel();
            }
        }


    }
}
