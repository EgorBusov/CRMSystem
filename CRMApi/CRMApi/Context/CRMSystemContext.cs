using CRMApi.Models;
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
    }
}
