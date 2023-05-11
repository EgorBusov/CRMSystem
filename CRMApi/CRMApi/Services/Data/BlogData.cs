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
            string extension = Path.GetExtension(formFile.FileName); //получаем формат файла
            if (extension.ToLower() == ".jpeg" ||  extension.ToLower() == ".jpg")
            {
                string fileName = Guid.NewGuid().ToString() + formFile.FileName;
                string path = Path.Combine(AppContext.BaseDirectory, "Pictures", "BlogPictures", fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);//загружаем картинку в поток
                }
                return fileName;
            }
            else { throw new Exception("Неверный формат"); }
        }
        /// <summary>
        /// Удаляет картинку
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task DeletePicture(string path)
        {
            File.Delete(Path.Combine(AppContext.BaseDirectory, "Pictures", "BlogPictures", path));
        }
        public async Task AddBlog(BlogModel model)
        {
            Blog blog = new Blog() { GuidPicture = await SavePicture(model.Picture),
                                     Description = model.Description, Title = model.Title};
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
            await DeletePicture(blog.GuidPicture);
            blog.GuidPicture = await SavePicture(model.Picture);
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
