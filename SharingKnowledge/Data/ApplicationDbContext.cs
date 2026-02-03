using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Models;

namespace SharingKnowledge.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext(options)
    {
        public virtual DbSet<OpenCourse> OpenCourses { get; set; } = null!;

        public virtual DbSet<CourseCategory> CourseCategories { get; set; } = null!;



    }
}
