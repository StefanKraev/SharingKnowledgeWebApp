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

            string ghostAdminId = "007e37ed-27e5-43cb-a8b7-a3b14d054f45";

            var admin = new Student
            {
                Id = ghostAdminId,
                UserName = "GhostAdmin",
                NormalizedUserName = "GHOSTADMIN",
                Email = "admin@sharingknowledge.com",
                NormalizedEmail = "ADMIN@SHARINGKNOWLEDGE.COM",
                FacultyNumber = "0MI0000000",
                EmailConfirmed = true,
                SecurityStamp = "3235650d-6e47-49f3-9d0a-04664879201a",
                ConcurrencyStamp = "86778f79-246e-4861-8271-6c589679199c",
                PasswordHash = "AQAAAAIAAYagAAAAEGVx7nHYW18d6vEjqAitwoxLyms6pEPIbLVGX7rivzEBJMpHtztkFthMsfbEaXgWvQ=="
            };

            entity.HasData(admin);
        }
    }
}
