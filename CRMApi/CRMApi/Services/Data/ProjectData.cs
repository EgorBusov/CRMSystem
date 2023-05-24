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
        private readonly IPictureManager _pictureManager;
        public ProjectData(CRMSystemContext context, IPictureManager pictureManager) 
        {
            _context = context;
            _pictureManager = pictureManager;
        }
        public async Task AddProject(ProjectModel model)
        {
            Project project = new Project()
            {
                GuidPicture = await _pictureManager.SavePicture(model.Picture, @"Pictures\ProjectPictures"),
                Description = model.Description,
                Title = model.Title
            };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProject(int idProject)
        {
            Project project = await _context.Projects.FirstOrDefaultAsync(b => b.Id == idProject) ?? throw new Exception("Запись не найдена");
            await _pictureManager.DeletePicture(project.GuidPicture, @"Pictures\ProjectPictures");
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
            Project project = await _context.Projects.FirstOrDefaultAsync(a => a.Id == model.Id) ?? throw new Exception("Запись не найдена");
            await _pictureManager.DeletePicture(project.GuidPicture, @"Pictures\ProjectPictures");
            project.GuidPicture = await _pictureManager.SavePicture(model.Picture, @"Pictures\ProjectPictures");
            project.Description = model.Description;
            project.Title = model.Title;
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
