using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.ViewModels.Courses;
using SharingKnowledge.Web.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SharingKnowledge.Services.Core
{
    public class OpenCoursesService : IOpenCoursesService
    {
        private readonly ApplicationDbContext context;

        public OpenCoursesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OpenCoursesMyCoursesViewModel>> GetAllCoursesAsync(string userId)
        {
            return await context
                .OpenCourses
                .AsNoTracking()
                .Include(c => c.EnrolledStudents)
                .OrderByDescending(oc => oc.StartDate)
                .Select(oc => new OpenCoursesMyCoursesViewModel
                {
                    Id = oc.Id,
                    Title = oc.Title,
                    Description = oc.Description.
                            Length > 100
                          ? oc.Description.Substring(0, 97) + "..."
                          : oc.Description,
                    StartDate = oc.StartDate,
                    ImageUrl = oc.ImageUrl,
                    CategoryName = oc.Category.Name,
                    CreatorId = oc.CreatorId,

                    IsEnrolled = userId != null && oc.EnrolledStudents.Any(s => s.Id == userId)
                })
                .ToListAsync();
        }

        public async Task<OpenCoursesDetailsViewModel?> GetCourseDetailsAsync(int id)
        {
            return await context
                .OpenCourses
                .AsNoTracking()
                .Where(oc => oc.Id == id)
                .Select(oc => new OpenCoursesDetailsViewModel
                {
                    Title = oc.Title,
                    Description = oc.Description,
                    StartDate = oc.StartDate,
                    ImageUrl = oc.ImageUrl,
                    CategoryName = oc.Category.Name,
                    AuthorEmail = oc.Creator.Email ?? "Email not found!"
                })
                .SingleOrDefaultAsync();
        }

        public async Task CreateCourseAsync(OpenCoursesCreateInputModel inputModel, string userId)
        {
            OpenCourse openCourse = new OpenCourse
            {
                Title = inputModel.Title,
                Description = inputModel.Description,
                StartDate = inputModel.StartDate,
                ImageUrl = inputModel.ImageUrl,
                CategoryId = inputModel.CategoryId,
                CreatorId = userId,
                EnrolledStudents = new List<Student>()
            };

            await context.OpenCourses.AddAsync(openCourse);
            await context.SaveChangesAsync();
        }

        public async Task<OpenCoursesCreateInputModel> CreateCourseInput()
        {
            OpenCoursesCreateInputModel openCoursesCreateInputModel = 
                new OpenCoursesCreateInputModel
            {
                Categories = await GetCategoriesAsync()
            };

            return openCoursesCreateInputModel;
        }

        public async Task<OpenCoursesCreateInputModel?> GetCourseForEditAsync(int id, string userId)
        {
            OpenCourse? openCourse = await context
                .OpenCourses
                .Include(oc => oc.Category)
                .SingleOrDefaultAsync(oc => oc.Id == id);

            if (openCourse == null)
            {
                return null;
            }

            if (userId == null || openCourse.CreatorId != userId)
            {
                return null;
            }

            OpenCoursesCreateInputModel inputModel = new OpenCoursesCreateInputModel
            {
                Title = openCourse.Title,
                Description = openCourse.Description,
                StartDate = openCourse.StartDate,
                ImageUrl = openCourse.ImageUrl,
                CategoryId = openCourse.CategoryId,
                Categories = await GetCategoriesAsync()
            };

            return inputModel;
        }

        public async Task<bool> EditCourseAsync(int id, OpenCoursesCreateInputModel inputModel, string userId)
        {
            OpenCourse ?openCourse = await context
                .OpenCourses
                .SingleOrDefaultAsync(oc => oc.Id == id);

            if (openCourse == null || openCourse.CreatorId != userId)
            {
                return false;
            }

            openCourse.Title = inputModel.Title;
            openCourse.Description = inputModel.Description;
            openCourse.StartDate = inputModel.StartDate;
            openCourse.ImageUrl = inputModel.ImageUrl;
            openCourse.CategoryId = inputModel.CategoryId;

            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<OpenCoursesDeleteViewModel?> GetCourseForDeleteAsync(int id, string userId)
        {
            OpenCourse? openCourse = await context
                .OpenCourses
                .Include(oc => oc.Category)
                .SingleOrDefaultAsync(oc => oc.Id == id);

            if (openCourse == null)
            {
                return null;
            }

            if (userId == null || openCourse.CreatorId != userId)
            {
                return null;
            }

            OpenCoursesDeleteViewModel viewModel = new OpenCoursesDeleteViewModel
            {
                Id = openCourse.Id,
                Title = openCourse.Title
            };

            return viewModel;
        }

        public async Task<bool> DeleteCourseAsync(int id, string userId)
        {
            OpenCourse ?course = await context
                .OpenCourses
                .FirstOrDefaultAsync(oc => oc.Id == id);

            if (course == null || course.CreatorId != userId)
            {
                return false;
            }

            context.OpenCourses.Remove(course);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Student?> GetStudentByIdAsync(string userId)
        {
            return await context
                .Users
                .OfType<Student>()
                .Include(s => s.EnrolledCourses)
                .ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(s => s.Id == userId);
        }

        public async Task<OpenCourse?> GetCourseByIdAsync(int id)
        {
            return await context.OpenCourses.FindAsync(id);
        }

        public async Task AddCourseToStudentAsync(OpenCourse course, Student student)
        {
           
            student.EnrolledCourses.Add(course);
            await context.SaveChangesAsync();
        }

        public async Task<ICollection<OpenCoursesMyCoursesViewModel>> GetAllStudentCourses(Student student)
        {
            return student
                .EnrolledCourses
                .Select(c => new OpenCoursesMyCoursesViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    ImageUrl = c.ImageUrl,
                    CategoryName = c.Category?.Name ?? "General",
                    CreatorId = c.CreatorId,
                    IsEnrolled = true
                }).ToList();
        }

        public async Task UnenrollStudentAsync(Student student, OpenCourse course)
        {
            if (student.EnrolledCourses.Any(c => c.Id == course.Id))
            {
                student.EnrolledCourses.Remove(course);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CourseCategory>> GetCategoriesAsync()
        {
            return await context.CourseCategories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await context.CourseCategories.AnyAsync(c => c.Id == categoryId);
        }
    }
}
