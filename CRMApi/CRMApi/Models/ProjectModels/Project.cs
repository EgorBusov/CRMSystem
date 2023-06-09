﻿namespace CRMApi.Models.ProjectModels
{
    /// <summary>
    /// Сделанный проект
    /// </summary>
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string GuidPicture { get; set; }
    }
}
