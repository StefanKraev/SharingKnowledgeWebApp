using SharingKnowledge.Data.Repository;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.Web.ViewModels.Courses;

namespace SharingKnowledge.Services.Core
{
    public class MyCoursesService : IMyCoursesService
    {
        private readonly ICourseRepository courseRepository;

        private readonly IStudentRepository studentRepository;

        public MyCoursesService(ICourseRepository courseRepository , IStudentRepository studentRepository)
        {
            this.courseRepository = courseRepository;
            this.studentRepository = studentRepository;
        }

        public async Task AddCourseToStudentAsync(OpenCourse course, Student student)
        {
            student.EnrolledCourses.Add(course);
            await courseRepository.SaveChangesAsync();
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

        public async Task<OpenCourse?> GetCourseByIdAsync(int id)
        {
            return await courseRepository.GetCourseByIdAsync(id);
        }

        public async Task<Student?> GetStudentByIdAsync(string userId)
        {
            return await studentRepository.GetStudentByIdAsync(userId);
        }

        public async Task UnenrollStudentAsync(Student student, OpenCourse course)
        {
            if (student.EnrolledCourses.Any(c => c.Id == course.Id))
            {
                student.EnrolledCourses.Remove(course);
                await courseRepository.SaveChangesAsync();
            }
        }
    }
}
