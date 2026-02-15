using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SharingKnowledge.ViewModels.Courses;
using SharingKnowledge.Models;
using SharingKnowledge.Web.ViewModels.Courses;

namespace SharingKnowledge.Services.Core.Interfaces
{
    public interface IOpenCoursesService
    {
        Task<IEnumerable<OpenCoursesMyCoursesViewModel>> GetAllCoursesAsync(string userId); //all

        Task<OpenCoursesDetailsViewModel?> GetCourseDetailsAsync(int id); //details

        Task CreateCourseAsync(OpenCoursesCreateInputModel inputModel, string userId); //create

        Task<OpenCoursesCreateInputModel> CreateCourseInput(); //create

        Task<OpenCoursesCreateInputModel?> GetCourseForEditAsync(int id, string userId); //edit

        Task<bool> EditCourseAsync(int id, OpenCoursesCreateInputModel inputModel, string userId); //edit

        Task<OpenCoursesDeleteViewModel?> GetCourseForDeleteAsync(int id, string userId); //delete

        Task<bool> DeleteCourseAsync(int id, string userId); // delete

        Task<IEnumerable<CourseCategory>> GetCategoriesAsync(); // getCategories

        Task<bool> CategoryExistsAsync(int categoryId); //existCategory

        Task<Student> GetStudentByIdAsync(string userId); //studentById

        Task<OpenCourse> GetCourseByIdAsync(int id); //courseById

        Task AddCourseToStudentAsync(OpenCourse course, Student student); //addCourseToStudent

        Task<ICollection<OpenCoursesMyCoursesViewModel>> GetAllStudentCourses(Student student); //DisplayMyCourses

        Task UnenrollStudentAsync(Student student, OpenCourse course); //unenrollCourse
    }
}
