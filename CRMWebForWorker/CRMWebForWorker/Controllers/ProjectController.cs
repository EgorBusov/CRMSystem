using CRMWebForWorker.ApiInteraction.ApiRequests;
using CRMWebForWorker.Models.BlogModels;
using CRMWebForWorker.Models.ProjectModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http;

namespace CRMWebForWorker.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectRequests _projectRequests;

        public ProjectController(ProjectRequests projectRequests)
        {
            _projectRequests = projectRequests;
        }

        /// <summary>
        /// Получение всех проектов
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.Route("GetProjects")]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> GetProjectModels()
        {
            try
            {
                IEnumerable<ProjectModel> models = await _projectRequests.GetProjectsRequest();
                List<ProjectToView> projectToViews = new List<ProjectToView>();
                foreach (var model in models)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        ProjectToView view = new ProjectToView()
                            { Id = model.Id, Description = model.Description, Title = model.Title };
                        await model.Picture.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();
                        var imageBase64 = Convert.ToBase64String(imageBytes);

                        view.Picture = imageBase64;
                        projectToViews.Add(view);
                    }
                }

                return View(projectToViews);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Получение проекта по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> GetProjectById(int id)
        {
            try
            {
                ProjectModel model = await _projectRequests.GetProjectByIdRequest(id);
                ProjectToView view = new ProjectToView()
                    { Id = model.Id, Title = model.Title, Description = model.Description };
                using (var memoryStream = new MemoryStream())
                {
                    await model.Picture.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    var imageBase64 = Convert.ToBase64String(imageBytes);

                    view.Picture = imageBase64;
                }

                return View(view);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Blog/GetBlogs");
            }
        }

        /// <summary>
        /// Добавление проекта
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult AddProject()
        {
            return View();
        }

        /// <summary>
        /// Добавление проекта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.Route("AddProject")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> AddBlog([FromForm] ProjectModel model)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);
                await _projectRequests.AddProjectRequest(model, token);
                return Redirect("/Project/GetProjects");
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Project/GetProjects");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Project/GetProjects");
            }
        }

        /// <summary>
        /// Редактирование проекта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> EditProject(int id)
        {
            try
            {
                ProjectModel model = await _projectRequests.GetProjectByIdRequest(id);
                EditProjectModel editModel = new EditProjectModel()
                    { Id = model.Id, Title = model.Title, Description = model.Description, Picture = model.Picture };

                using (var memoryStream = new MemoryStream())
                {
                    await model.Picture.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    var imageBase64 = Convert.ToBase64String(imageBytes);
                    editModel.StringPicture = imageBase64;
                }

                return View(editModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Редактирование проекта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> EditProject([FromForm] EditProjectModel editModel)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);
                ProjectModel model = new ProjectModel()
                {
                    Id = editModel.Id,
                    Description = editModel.Description,
                    Title = editModel.Title,
                    Picture = editModel.Picture
                };
                await _projectRequests.EditProjectRequest(model, token);
                return Redirect("/Project/GetProjects");

            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Project/GetProjects");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Project/GetProjects");
            }
        }

        /// <summary>
        /// Удаление проекта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);
                await _projectRequests.DeleteProjectRequest(id, token);
                return Redirect("/Project/GetProjects");

            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Project/GetProjects");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Project/GetProjects");
            }
        }
    }
}
