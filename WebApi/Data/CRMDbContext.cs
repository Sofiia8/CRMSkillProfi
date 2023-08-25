using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class CRMDbContext: IdentityDbContext<User>
    {
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationStatus> ApplicationStates { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Network> Networks { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<MainSettings> MainSettings { get; set; }
        public CRMDbContext(DbContextOptions<CRMDbContext> options):
            base(options)
        {
            bool isCreated = Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<WebApi.Models.Application>()
                .HasOne (a => a.ApplicationStatus)
                .WithMany(s => s.Applications)
                .HasForeignKey(a => a.ApplicationStatusId);
            
        }
    }
}
