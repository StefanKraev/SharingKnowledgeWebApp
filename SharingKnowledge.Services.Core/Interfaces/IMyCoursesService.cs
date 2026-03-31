using SharingKnowledge.Models;
using SharingKnowledge.Web.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Services.Core.Interfaces
{
    public interface IMyCoursesService
    {
        Task<Student> GetStudentByIdAsync(string userId);

        Task<ICollection<MyCoursesViewModel>> GetAllStudentCourses(Student student);

        Task<OpenCourse> GetCourseByIdAsync(int id);

        Task AddCourseToStudentAsync(OpenCourse course, Student student);

        Task UnenrollStudentAsync(Student student, OpenCourse course);
    }
}
