using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Models;
using CRMApi.Models.ModelsApi;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CRMApi.Services.Data
{
    public class BlogData : IBlogData
    {
        private readonly CRMSystemContext _context;       
        public BlogData(CRMSystemContext context) 
        {
            _context = context;
        }
        /// <summary>
        /// Сохраняет картинку блога
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        private async Task<string> SavePicture(IFormFile formFile)
        {
            string fileName = Guid.NewGuid().ToString();
            string path = Path.Combine("Pictures/BlogPictures", fileName);
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
            return fileName;
        }
        /// <summary>
        /// Удаляет картинку
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task DeletePicture(string path)
        {
            File.Delete($"Pictures/BlogPictures/{path}.jpg");
        }
        public async Task AddBlog(IFormFile formFile, string title, string description)
        {
            Blog blog = new Blog() { GuidPicture = await SavePicture(formFile),
                                     Description = description, Title = title};
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteBlog(int idBlog)
        {
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == idBlog) ?? throw new Exception("Запись не найдена");
            await DeletePicture(blog.GuidPicture);
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
                blogModel.Picture = await File.ReadAllBytesAsync($"Pictures/BlogPictures/{blog.GuidPicture}.jpg");
                blogModels.Add(blogModel);
            }
            return blogModels;
        }
        public async Task EditBlog(Blog b,IFormFile formFile)
        {
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(a => a.Id == b.Id) ?? throw new Exception("Blog не найден");
            blog.GuidPicture = await SavePicture(formFile);
            blog.Description = b.Description;
            blog.Title = b.Title;
            await _context.SaveChangesAsync();
        }
        public async Task<BlogModel> GetBlogById(int id)
        {
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(a => a.Id == id) ?? throw new Exception("Blog не найден");
            BlogModel blogModel = new BlogModel() { Id = blog.Id, Description = blog.Description, Title = blog.Title };
            blogModel.Picture = await File.ReadAllBytesAsync($"Pictures/BlogPictures/{blog.GuidPicture}.jpg");
            return blogModel;
        }

    }
}
