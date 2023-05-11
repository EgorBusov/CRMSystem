using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Models.ProjectModels;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System.IO;
using System.Reflection.Metadata;

namespace CRMApi.Services.Data
{
    public class ProjectData : IProjectData
    {
        private readonly CRMSystemContext _context;
        public ProjectData(CRMSystemContext context) 
        {
            _context = context;
        }
        /// <summary>
        /// Сохранение картинки проекта
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        private async Task<string> SavePicture(IFormFile formFile)
        {
            string extension = Path.GetExtension(formFile.FileName); //получаем формат файла
            if (extension.ToLower() == ".jpeg" || extension.ToLower() == ".jpg")
            {
                string fileName = Guid.NewGuid().ToString() + formFile.FileName;
                string path = Path.Combine(AppContext.BaseDirectory, "Pictures", "ProjectPictures", fileName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
                return fileName;
            }
            else { throw new Exception("Неверный формат"); }
            
        }
        /// <summary>
        /// Удаление картинки проекта
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task DeletePicture(string path)
        {
            File.Delete(Path.Combine(AppContext.BaseDirectory, "Pictures", "ProjectPictures", path));
        }
        public async Task AddProject(ProjectModel model)
        {
            Project project = new Project()
            {
                GuidPicture = await SavePicture(model.Picture),
                Description = model.Description,
                Title = model.Title
            };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProject(int idProject)
        {
            Project project = await _context.Projects.FirstOrDefaultAsync(b => b.Id == idProject) ?? throw new Exception("Запись не найдена");
            await DeletePicture(project.GuidPicture);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProjectModel>> GetProjectModels()
        {
            List<Project> projects = await _context.Projects.ToListAsync();
            List<ProjectModel> projectModels = new List<ProjectModel>();
            foreach (Project project in projects)
            {
                ProjectModel projectModel = new ProjectModel() { Id = project.Id, Description = project.Description, Title = project.Title };
                byte[] bytes = await File.ReadAllBytesAsync(Path.Combine(AppContext.BaseDirectory, 
                                                                         "Pictures", "ProjectPictures", project.GuidPicture));
                projectModel.Picture = new FormFile(new MemoryStream(bytes), 0, bytes.Length, null, project.GuidPicture)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };
                projectModels.Add(projectModel);
            }
            return projectModels;
        }
        public async Task EditProject(ProjectModel model)
        {
            Project blog = await _context.Projects.FirstOrDefaultAsync(a => a.Id == model.Id) ?? throw new Exception("Запись не найдена");
            await DeletePicture(blog.GuidPicture);
            blog.GuidPicture = await SavePicture(model.Picture);
            blog.Description = model.Description;
            blog.Title = model.Title;
            await _context.SaveChangesAsync();
        }
        public async Task<ProjectModel> GetProjectById(int id)
        {
            Project project = await _context.Projects.FirstOrDefaultAsync(a => a.Id == id) ?? throw new Exception("Запись не найден");
            ProjectModel projectModel = new ProjectModel() { Id = project.Id, Description = project.Description, Title = project.Title };
            byte[] bytes = await File.ReadAllBytesAsync(Path.Combine(AppContext.BaseDirectory,
                                                                         "Pictures", "ProjectPictures", project.GuidPicture));
            projectModel.Picture = new FormFile(new MemoryStream(bytes), 0, bytes.Length, null, project.GuidPicture)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
            return projectModel;
        }
    }
}
