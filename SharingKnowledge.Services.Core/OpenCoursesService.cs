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

        public Task<IEnumerable<OpenCoursesAllViewModel>> GetAllCoursesAsync()
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

        public Task<OpenCoursesDetailsViewModel?> GetCourseDetailsAsync(int id)
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
