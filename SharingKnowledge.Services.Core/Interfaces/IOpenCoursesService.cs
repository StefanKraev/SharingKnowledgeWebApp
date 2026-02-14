using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SharingKnowledge.ViewModels.Courses;
using SharingKnowledge.Models;

namespace SharingKnowledge.Services.Core.Interfaces
{
    public interface IOpenCoursesService
    {
        Task<IEnumerable<OpenCoursesAllViewModel>> GetAllCoursesAsync(); //all

        Task<OpenCoursesDetailsViewModel?> GetCourseDetailsAsync(int id); //details

        Task CreateCourseAsync(OpenCoursesCreateInputModel inputModel, string userId); //create

        Task<OpenCoursesCreateInputModel> CreateCourseInput(); //create

        Task<OpenCoursesCreateInputModel?> GetCourseForEditAsync(int id, string userId); //edit

        Task<bool> EditCourseAsync(int id, OpenCoursesCreateInputModel inputModel, string userId); //edit

        Task<OpenCoursesDeleteViewModel?> GetCourseForDeleteAsync(int id, string userId); //delete

        Task<bool> DeleteCourseAsync(int id, string userId); // delete

        Task<IEnumerable<CourseCategory>> GetCategoriesAsync(); // getCategories

        Task<bool> CategoryExistsAsync(int categoryId); //existCategory
    }
}
