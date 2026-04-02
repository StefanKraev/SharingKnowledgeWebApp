using Microsoft.AspNetCore.Identity;
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
                             //be in conflict with IdentityUser::id

            entity
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            string ghostAdminUserId = "007e37ed-27e5-43cb-a8b7-a3b14d054f45";

            var adminProfile = new Student
            {
                Id = 1, 
                UserId = ghostAdminUserId,
                FacultyNumber = "0MI0000000"
            };

            entity.HasData(adminProfile);
        }
    }
}
