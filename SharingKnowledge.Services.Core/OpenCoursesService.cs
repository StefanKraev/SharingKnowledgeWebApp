using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.ViewModels.Courses;
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

        public async Task<IEnumerable<OpenCoursesAllViewModel>> GetAllCoursesAsync()
        {
            return await context
                .OpenCourses
                .AsNoTracking()
                .OrderByDescending(oc => oc.StartDate)
                .Select(oc => new OpenCoursesAllViewModel
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
                    CreatorId = oc.CreatorId
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

        public Task<bool> DeleteCourseAsync(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<OpenCoursesDeleteViewModel?> GetCourseForDeleteAsync(int id, string userId)
        {
            throw new NotImplementedException();
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
