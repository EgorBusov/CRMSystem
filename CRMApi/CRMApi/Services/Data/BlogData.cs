using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Models.BlogModels;
using CRMApi.Models.ProjectModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace CRMApi.Services.Data
{
    public class BlogData : IBlogData
    {
        private readonly IPictureManager _pictureManager;
        private readonly CRMSystemContext _context;
        private readonly string baseUrl;
        public BlogData(CRMSystemContext context, IPictureManager pictureManager, IConfiguration configuration) 
        {
            _context = context;
            _pictureManager = pictureManager;
            baseUrl = configuration.GetValue<string>("BaseUrl:Url");
        }
        public async Task AddBlog(BlogModel model)
        {
            Blog blog = new Blog() { GuidPicture = await _pictureManager.SavePicture(model.Picture),
                                     Description = model.Description, Title = model.Title};
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteBlog(int idBlog)
        {
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == idBlog) ?? throw new Exception("Запись не найдена");
            await _pictureManager.DeletePicture(blog.GuidPicture);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<BlogPath>> GetBlogs()
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            List<BlogPath> blogPaths = new List<BlogPath>();
            foreach (Blog blog in blogs)
            {
                BlogPath blogPath = new BlogPath() { Id = blog.Id, Description = blog.Description, Title = blog.Title };
                blogPath.Picture = $"{baseUrl}/Resource/GetPicture/{blog.GuidPicture}";
                blogPaths.Add(blogPath);
            }
            return blogPaths;
        }
        public async Task EditBlog(BlogModel model)
        {

            Blog blog = await _context.Blogs.FirstOrDefaultAsync(a => a.Id == model.Id) ?? throw new Exception("Blog не найден");
            if (model.Picture.Length > 0)
            {
                await _pictureManager.DeletePicture(blog.GuidPicture);
                blog.GuidPicture = await _pictureManager.SavePicture(model.Picture);
            }
            blog.Description = model.Description;
            blog.Title = model.Title;
            await _context.SaveChangesAsync();
        }
        public async Task<BlogPath> GetBlogById(int id)
        {
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(a => a.Id == id) ?? throw new Exception("Blog не найден");
            BlogPath blogPath = new BlogPath() { Id = blog.Id, Description = blog.Description, Title = blog.Title };
            blogPath.Picture = $"{baseUrl}/Resource/GetPicture/{blog.GuidPicture}";
            return blogPath;
        }

    }
}
