using CRMWebForWorker.ApiInteraction.ApiRequests;
using CRMWebForWorker.Models.BlogModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http;

namespace CRMWebForWorker.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogRequests _blogRequests;

        public BlogController(BlogRequests blogRequests)
        {
            _blogRequests = blogRequests;
        }

        [Microsoft.AspNetCore.Mvc.Route("GetBlogs")]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        /// <summary>
        /// Получение всех блогов
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetBlogModels()
        {
            try
            {
                IEnumerable<BlogModel> models = await _blogRequests.GetBlogModelsRequest();
                List<BlogToView> blogToViews = new List<BlogToView>();
                foreach (var model in models)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        BlogToView view = new BlogToView()
                            { Id = model.Id, Description = model.Description, Title = model.Title };
                        await model.Picture.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();
                        var imageBase64 = Convert.ToBase64String(imageBytes);

                        view.Picture = imageBase64;
                        blogToViews.Add(view);
                    }
                }

                return View(blogToViews);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        /// <summary>
        /// Получение блога по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetBlogById(int id)
        {
            try
            {
                BlogModel model = await _blogRequests.GetBlogByIdRequest(id);
                BlogToView view = new BlogToView()
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
                return View();
            }
        }

        /// <summary>
        /// Добавление блога
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult AddBlog()
        {
            return View();
        }

        /// <summary>
        /// Добавление блога
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.Route("AddBlog")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> AddBlog([FromForm] BlogModel model)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);
                await _blogRequests.AddBlogRequest(model, token);
                return Redirect("/Blog/GetBlogs");
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Blog/GetBlogs");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Blog/GetBlogs");
            }
        }

        /// <summary>
        /// Редактирование блога
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> EditBlog(int id)
        {
            try
            {
                BlogModel model = await _blogRequests.GetBlogByIdRequest(id);
                EditBlogModel editModel = new EditBlogModel()
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
        /// Редактирование блога
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> EditBlog([FromForm] EditBlogModel editModel)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);
                BlogModel model = new BlogModel()
                {
                    Id = editModel.Id, Description = editModel.Description, Title = editModel.Title,
                    Picture = editModel.Picture
                };
                await _blogRequests.EditBlogRequest(model, token);
                return Redirect("/Blog/GetBlogs");

            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Blog/GetBlogs");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Blog/GetBlogs");
            }
        }

        /// <summary>
        /// Удаление блога
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            try
            {
                string token = Request.Cookies["jwt"] ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);
                await _blogRequests.DeleteBlogRequest(id, token);
                return Redirect("/Blog/GetBlogs");

            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Redirect("/Account/Login");
                }
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Blog/GetBlogs");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Redirect("/Blog/GetBlogs");
            }
        }

    }
}
