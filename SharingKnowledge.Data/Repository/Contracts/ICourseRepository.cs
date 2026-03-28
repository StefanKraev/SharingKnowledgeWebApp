using SharingKnowledge.Models;
using SharingKnowledge.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Data.Repository.Contracts
{
    public interface ICourseRepository
    {
        Task<IEnumerable<OpenCourse>> GetAllOpenCoursesAsync();

        Task<bool> AddOpenCourseAsync(OpenCourse openCourse);

        Task<OpenCoursesDetailsViewModel> GetOpenCourseByIdAsync(int openCourseId);
    }
}
