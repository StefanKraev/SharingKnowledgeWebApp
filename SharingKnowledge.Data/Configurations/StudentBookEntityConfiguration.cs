using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharingKnowledge.Data.Models;

namespace SharingKnowledge.Data.Configurations
{
    public class StudentBookEntityConfiguration : IEntityTypeConfiguration<StudentBook>
    {
        public void Configure(EntityTypeBuilder<StudentBook> entity)
        {
            entity
                .HasKey(sb => new { sb.StudentId, sb.BookId });

            entity
                .HasOne(sb => sb.Student)
                .WithMany(s => s.StudentBooks)
                .HasForeignKey(sb => sb.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(sb => sb.Book)
                .WithMany(b => b.BookStudents)
                .HasForeignKey(sb => sb.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
