using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharingKnowledge.Models;

namespace SharingKnowledge.Data.Configurations
{
    public class CourseCategoryEntityConfiguration : IEntityTypeConfiguration<CourseCategory>
    {
        private readonly ICollection<CourseCategory> categories = new List<CourseCategory>
        {
            new CourseCategory { Id = 1, Name = "Programming" },
            new CourseCategory { Id = 2, Name = "Databases" },
            new CourseCategory { Id = 3, Name = "Web Development" },
            new CourseCategory { Id = 4, Name = "Data Science" },
            new CourseCategory { Id = 5, Name = "Mobile Development" },
            new CourseCategory { Id = 6, Name = "Cloud Computing" },
            new CourseCategory { Id = 7, Name = "DevOps" },
            new CourseCategory { Id = 8, Name = "Cybersecurity" },
            new CourseCategory { Id = 9, Name = "AI & Machine Learning" },
            new CourseCategory { Id = 10, Name = "Game Development" }
        };

        public void Configure(EntityTypeBuilder<CourseCategory> entity)
        {
            entity.HasData(categories);
        }
    }
}
