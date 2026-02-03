using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SharingKnowledge.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenCourses_CourseCategories_CategoryId",
                table: "OpenCourses");

            migrationBuilder.InsertData(
                table: "CourseCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Programming" },
                    { 2, "Databases" },
                    { 3, "Web Development" },
                    { 4, "Data Science" },
                    { 5, "Mobile Development" },
                    { 6, "Cloud Computing" },
                    { 7, "DevOps" },
                    { 8, "Cybersecurity" },
                    { 9, "AI & Machine Learning" },
                    { 10, "Game Development" }
                });

            migrationBuilder.InsertData(
                table: "OpenCourses",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "StartDate", "Title" },
                values: new object[,]
                {
                    { 1, 1, "A beginner-friendly course covering C# fundamentals, types, control flow and basic OOP concepts.", "https://example.com/images/csharp-intro.png", new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Introduction to C#" },
                    { 2, 1, "Build dynamic web applications using Razor Pages, routing, model binding and forms.", "https://example.com/images/razor-pages.png", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core Razor Pages" },
                    { 3, 2, "Learn EF Core fundamentals: DbContext, migrations, querying, relationships and seeding data.", "https://example.com/images/efcore.png", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Entity Framework Core Essentials" },
                    { 4, 3, "Covers HTML structure, semantic tags, CSS layout and responsive design principles.", "https://example.com/images/html-css.png", new DateTime(2026, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Frontend Basics with HTML & CSS" },
                    { 5, 3, "Introduction to JavaScript language features, DOM manipulation and basic async patterns.", "https://example.com/images/js-fundamentals.png", new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "JavaScript Fundamentals" },
                    { 6, 2, "Principles and practice of unit testing in .NET with xUnit, mocking and testable design.", "https://example.com/images/dotnet-testing.png", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unit Testing in .NET" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OpenCourses_CourseCategories_CategoryId",
                table: "OpenCourses",
                column: "CategoryId",
                principalTable: "CourseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenCourses_CourseCategories_CategoryId",
                table: "OpenCourses");

            migrationBuilder.DeleteData(
                table: "CourseCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CourseCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CourseCategories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CourseCategories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CourseCategories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CourseCategories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CourseCategories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "OpenCourses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OpenCourses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OpenCourses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OpenCourses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OpenCourses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OpenCourses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CourseCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CourseCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CourseCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenCourses_CourseCategories_CategoryId",
                table: "OpenCourses",
                column: "CategoryId",
                principalTable: "CourseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
