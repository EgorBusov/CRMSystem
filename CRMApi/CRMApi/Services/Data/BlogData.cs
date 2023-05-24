using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Models.BlogModels;
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
        public BlogData(CRMSystemContext context, IPictureManager pictureManager) 
        {
            _context = context;
            _pictureManager = pictureManager;
        }
        public async Task AddBlog(BlogModel model)
        {
            Blog blog = new Blog() { GuidPicture = await _pictureManager.SavePicture(model.Picture, @"Pictures\BlogPictures"),
                                     Description = model.Description, Title = model.Title};
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteBlog(int idBlog)
        {
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == idBlog) ?? throw new Exception("Запись не найдена");
            await _pictureManager.DeletePicture(blog.GuidPicture, @"Pictures\BlogPictures");
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<BlogModel>> GetBlogModels()
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            List<BlogModel> blogModels = new List<BlogModel>();
            foreach (Blog blog in blogs)
            {
                BlogModel blogModel = new BlogModel() { Id = blog.Id, Description = blog.Description, Title = blog.Title };
                byte[] bytes = await File.ReadAllBytesAsync(Path.Combine(AppContext.BaseDirectory,
                                                                        "Pictures", "BlogPictures", blog.GuidPicture));
                blogModel.Picture = new FormFile(new MemoryStream(bytes), 0, bytes.Length, null, blog.GuidPicture)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };
                blogModels.Add(blogModel);
            }
            return blogModels;
        }
        public async Task EditBlog(BlogModel model)
        {

            Blog blog = await _context.Blogs.FirstOrDefaultAsync(a => a.Id == model.Id) ?? throw new Exception("Blog не найден");
            await _pictureManager.DeletePicture(blog.GuidPicture, @"Pictures\BlogPictures");
            blog.GuidPicture = await _pictureManager.SavePicture(model.Picture, @"Pictures\BlogPictures");
            blog.Description = model.Description;
            blog.Title = model.Title;
            await _context.SaveChangesAsync();
        }
        public async Task<BlogModel> GetBlogById(int id)
        {
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(a => a.Id == id) ?? throw new Exception("Blog не найден");
            BlogModel blogModel = new BlogModel() { Id = blog.Id, Description = blog.Description, Title = blog.Title };
            byte[] bytes = await File.ReadAllBytesAsync(Path.Combine(AppContext.BaseDirectory, "Pictures", "BlogPictures", blog.GuidPicture));
            blogModel.Picture = new FormFile(new MemoryStream(bytes), 0, bytes.Length, null, blog.GuidPicture)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
            return blogModel;
        }

    }
}
