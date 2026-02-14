using SharingKnowledge.ViewModels.Courses;

namespace SharingKnowledge.Services.Core.Interfaces
{
    public interface IOpenCoursesService
    {
        Task<IEnumerable<OpenCoursesAllViewModel>> GetAllOpenCoursesAsync();
    }
}
