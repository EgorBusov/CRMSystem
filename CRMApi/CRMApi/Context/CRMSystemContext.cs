﻿using CRMApi.Models;
using CRMApi.Models.AccountModels;
using CRMApi.Models.BlogModels;
using CRMApi.Models.ProjectModels;
using CRMApi.Models.ResourceModels;
using Microsoft.EntityFrameworkCore;

namespace CRMApi.Context
{
    public class CRMSystemContext : DbContext
    {
        public CRMSystemContext(DbContextOptions<CRMSystemContext> options)
        : base(options)
        {

        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Header> Phrases { get; set; }
        public DbSet<Button> Buttons { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<MainPageContent> MainPageContents { get; set; }
        public DbSet<OurInformation> OurInformation { get; set; }
    }
}
