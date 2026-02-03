using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharingKnowledge.Models;

namespace SharingKnowledge.Data.Configurations
{
    public class OpenCourseEntityConfiguration : IEntityTypeConfiguration<OpenCourse>
    {
        private readonly ICollection<OpenCourse> _courses = new List<OpenCourse>
        {
            new OpenCourse
            {
                Id = 1,
                Title = "Introduction to C#",
                Description = "A beginner-friendly course covering C# fundamentals, types, control flow and basic OOP concepts.",
                StartDate = new DateTime(2026, 03, 15),
                ImageUrl = "https://example.com/images/csharp-intro.png",
                CategoryId = 1
            },
            new OpenCourse
            {
                Id = 2,
                Title = "ASP.NET Core Razor Pages",
                Description = "Build dynamic web applications using Razor Pages, routing, model binding and forms.",
                StartDate = new DateTime(2026, 04, 01),
                ImageUrl = "https://example.com/images/razor-pages.png",
                CategoryId = 1
            },
            new OpenCourse
            {
                Id = 3,
                Title = "Entity Framework Core Essentials",
                Description = "Learn EF Core fundamentals: DbContext, migrations, querying, relationships and seeding data.",
                StartDate = new DateTime(2026, 05, 05),
                ImageUrl = "https://example.com/images/efcore.png",
                CategoryId = 2
            },
            new OpenCourse
            {
                Id = 4,
                Title = "Frontend Basics with HTML & CSS",
                Description = "Covers HTML structure, semantic tags, CSS layout and responsive design principles.",
                StartDate = new DateTime(2026, 03, 28),
                ImageUrl = "https://example.com/images/html-css.png",
                CategoryId = 3
            },
            new OpenCourse
            {
                Id = 5,
                Title = "JavaScript Fundamentals",
                Description = "Introduction to JavaScript language features, DOM manipulation and basic async patterns.",
                StartDate = new DateTime(2026, 04, 20),
                ImageUrl = "https://example.com/images/js-fundamentals.png",
                CategoryId = 3
            },
            new OpenCourse
            {
                Id = 6,
                Title = "Unit Testing in .NET",
                Description = "Principles and practice of unit testing in .NET with xUnit, mocking and testable design.",
                StartDate = new DateTime(2026, 06, 01),
                ImageUrl = "https://example.com/images/dotnet-testing.png",
                CategoryId = 2
            }
        };

        public void Configure(EntityTypeBuilder<OpenCourse> entity)
        {
            entity
                .HasOne(c => c.Category)    
                .WithMany(cat => cat.Courses) 
                .HasForeignKey(c => c.CategoryId) 
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasData(_courses);
        }
    }
}
