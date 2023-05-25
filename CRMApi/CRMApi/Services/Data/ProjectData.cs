using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Models.ProjectModels;
using CRMApi.Models.ResourceModels;
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
        private readonly string baseUrl;
        public ProjectData(CRMSystemContext context, IPictureManager pictureManager, IConfiguration configuration) 
        {
            _context = context;
            _pictureManager = pictureManager;
            baseUrl = configuration.GetValue<string>("BaseUrl:Url");
        }
        public async Task AddProject(ProjectModel model)
        {
            Project project = new Project()
            {
                GuidPicture = await _pictureManager.SavePicture(model.Picture),
                Description = model.Description,
                Title = model.Title
            };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProject(int idProject)
        {
            Project project = await _context.Projects.FirstOrDefaultAsync(b => b.Id == idProject) ?? throw new Exception("Запись не найдена");
            await _pictureManager.DeletePicture(project.GuidPicture);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProjectPath>> GetProjects()
        {
            List<Project> projects = await _context.Projects.ToListAsync();
            List<ProjectPath> projectPaths = new List<ProjectPath>();
            foreach (Project project in projects)
            {
                ProjectPath projectPath = new ProjectPath() { Id = project.Id, Description = project.Description, Title = project.Title };
                projectPath.Picture = $"{baseUrl}/Resource/GetPicture/{project.GuidPicture}";
                projectPaths.Add(projectPath);
            }
            return projectPaths;
        }
        public async Task EditProject(ProjectModel model)
        {
            Project project = await _context.Projects.FirstOrDefaultAsync(a => a.Id == model.Id) ?? throw new Exception("Запись не найдена");
            if (model.Picture.Length > 0)
            {
                await _pictureManager.DeletePicture(project.GuidPicture);
                project.GuidPicture = await _pictureManager.SavePicture(model.Picture);
            }
            project.Description = model.Description;
            project.Title = model.Title;
            await _context.SaveChangesAsync();
        }
        public async Task<ProjectPath> GetProjectById(int id)
        {
            Project project = await _context.Projects.FirstOrDefaultAsync(a => a.Id == id) ?? throw new Exception("Запись не найден");
            ProjectPath projectPath = new ProjectPath() { Id = project.Id, Description = project.Description, Title = project.Title };
            projectPath.Picture = $"{baseUrl}/Resource/GetPicture/{project.GuidPicture}";
            return projectPath;
        }
    }
}
