using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharingKnowledge.Models;
using static SharingKnowledge.Common.ValidationConstrains;

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
                .Property(s => s.FacultyNumber)
                .IsRequired()
                .HasMaxLength(StudentFNMaxLen);

            entity
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            //Hardcoding student as seed data for testing purposes.
            entity.HasData(new Student
            {
                Id = 1,
                UserId = "d5812fbc-b5f3-46c1-8eb1-e6f817687dab",
                FacultyNumber = "0MI0000000"
            });
        }
    }
}
