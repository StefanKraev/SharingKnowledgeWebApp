using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Models;

namespace SharingKnowledge.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<Student>(options)
    {
        public virtual DbSet<OpenCourse> OpenCourses { get; set; } = null!;

        public virtual DbSet<CourseCategory> CourseCategories { get; set; } = null!;

        //DbSet for model:Student is not added to avoid double mapping

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
