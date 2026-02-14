using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.ViewModels.Courses;
using System;
using System.Collections.Generic;
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

        public Task<bool> CategoryExistsAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task CreateCourseAsync(OpenCoursesCreateInputModel inputModel, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCourseAsync(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditCourseAsync(int id, OpenCoursesCreateInputModel inputModel, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OpenCoursesAllViewModel>> GetAllOpenCoursesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CourseCategory>> GetCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OpenCoursesDeleteViewModel?> GetCourseForDeleteAsync(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<OpenCoursesCreateInputModel?> GetCourseForEditAsync(int id, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
