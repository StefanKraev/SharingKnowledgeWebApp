using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharingKnowledge.Models;

namespace SharingKnowledge.Data.Configurations
{
    public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> entity)
        {
            entity
                .HasIndex(s => s.FacultyNumber) //for fast searching of student.
                .IsUnique(); //cannot be written as an attribute [key] as it will
                             //be in conflict with IdentitiyUser::id

        }
    }
}
