using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SharingKnowledge.Models;
using SharingKnowledge.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Data.Repository.Contracts
{
    public interface ICourseRepository
    {
        IQueryable<OpenCourse> GetAllOpenCoursesQuery();

        Task<bool> AddOpenCourseAsync(OpenCourse openCourse);

        Task<OpenCourse> GetOpenCourseByIdAsync(int openCourseId);

        Task<IEnumerable<CourseCategory>> GetAllCategories();

        Task<bool> ExistsCategoryAsync(int categoryId);

        Task<OpenCourse> GetCourseByIdForEditAsync(int courseId);

        Task<OpenCourse> GetCourseForUpdateAsync(int courseId);

        Task<OpenCourse> GetCourseForDeleteAsync(int courseId);

        Task<bool> DeleteCourseAsync(OpenCourse openCourse);

        Task<OpenCourse?> GetCourseByIdAsync(int id);

        Task<bool> SaveChangesAsync();
    }
}
