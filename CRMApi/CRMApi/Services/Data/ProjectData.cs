using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Models;
using CRMApi.Models.ModelsApi;
using Microsoft.EntityFrameworkCore;

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
            string fileName = Guid.NewGuid().ToString();
            string path = Path.Combine("Pictures/ProjectPictures", fileName);
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
            return fileName;
        }
        /// <summary>
        /// Удаление картинки проекта
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task DeletePicture(string path)
        {
            File.Delete($"Pictures/ProjectPictures/{path}.jpg");
        }
        public async Task AddProject(IFormFile formFile, string title, string description)
        {
            Project project = new Project()
            {
                GuidPicture = await SavePicture(formFile),
                Description = description,
                Title = title
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
                projectModel.Picture = await File.ReadAllBytesAsync($"Pictures/ProjectPictures/{project.GuidPicture}.jpg");
                projectModels.Add(projectModel);
            }
            return projectModels;
        }
        public async Task EditProject(Project p, IFormFile formFile)
        {
            Project blog = await _context.Projects.FirstOrDefaultAsync(a => a.Id == p.Id) ?? throw new Exception("Запись не найдена");
            blog.GuidPicture = await SavePicture(formFile);
            blog.Description = p.Description;
            blog.Title = p.Title;
            await _context.SaveChangesAsync();
        }
        public async Task<ProjectModel> GetProjectById(int id)
        {
            Project blog = await _context.Projects.FirstOrDefaultAsync(a => a.Id == id) ?? throw new Exception("Запись не найден");
            ProjectModel projectModel = new ProjectModel() { Id = blog.Id, Description = blog.Description, Title = blog.Title };
            projectModel.Picture = await File.ReadAllBytesAsync($"Pictures/BlogPictures/{blog.GuidPicture}.jpg");
            return projectModel;
        }
    }
}
