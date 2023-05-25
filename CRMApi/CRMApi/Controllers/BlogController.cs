using CRMApi.Interfaces;
using CRMApi.Models;
using CRMApi.Models.BlogModels;
using CRMApi.Services.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;
using System.Data;
using System.IO;
using System.Reflection.Metadata;

namespace CRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogData _blogData;
        public BlogController(IBlogData blogData)
        {
            _blogData = blogData;
        }      
        /// <summary>
        /// Добавление блога
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddBlog")]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddBlog([FromForm] BlogModel model)
        {
            try
            {
                if (ModelState.IsValid) { await _blogData.AddBlog(model); return Ok(); }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Удаление блога
        /// </summary>
        /// <param name="idBlog"></param>
        /// <returns></returns>
        [Route("DeleteBlog/{id}")]
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            try
            {
                await _blogData.DeleteBlog(id); 
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Получение всех блогов
        /// </summary>
        /// <returns></returns>
        [Route("GetBlogs")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<BlogPath>> GetBlogs()
        {
            return await _blogData.GetBlogs();
        }
        /// <summary>
        /// Изменение блога
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("EditBlog")]
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditBlog([FromForm] BlogModel model)
        {
            try
            {
                if (ModelState.IsValid) { await _blogData.EditBlog(model); return Ok(); }
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
        [Route("GetBlogById/{id}")]
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<BlogPath> GetBlogById(int id)
        {
            try
            {
                return await _blogData.GetBlogById(id);
            }
            catch (Exception ex)
            {
                return new BlogPath();
            }
        }
    }
}
