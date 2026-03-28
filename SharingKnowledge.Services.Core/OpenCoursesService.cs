using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data;
using SharingKnowledge.Data.Repository;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Common;
using SharingKnowledge.Services.Common.Exceptions;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.ViewModels.Courses;
using SharingKnowledge.Web.ViewModels.Courses;
using System.Data;


namespace SharingKnowledge.Services.Core
{ 
    public class OpenCoursesService : IOpenCoursesService
    {
        private readonly ICourseRepository courseRepository;

        private readonly IStudentRepository studentRepository;

        public OpenCoursesService(ICourseRepository courseRepository, IStudentRepository studentRepository)
        {
            this.courseRepository = courseRepository;
            this.studentRepository = studentRepository;
        }

        public async Task<IEnumerable<OpenCoursesMyCoursesViewModel>> GetAllCoursesAsync(string userId)
        {
            IEnumerable<OpenCourse> allOpenCourses = await courseRepository.GetAllOpenCoursesAsync();

            return allOpenCourses
                .Select(oc => new OpenCoursesMyCoursesViewModel
                {
                    Id = oc.Id,
                    Title = oc.Title,
                    Description = oc.Description.Length > 100
                        ? oc.Description.Substring(0, 97) + "..."
                        : oc.Description,
                    StartDate = oc.StartDate,
                    ImageUrl = oc.ImageUrl,
                    CategoryName = oc.Category?.Name ?? "General",
                    CreatorId = oc.CreatorId,
                    IsEnrolled = userId != null && oc.EnrolledStudents.Any(s => s.Id == userId)
                });
        }

        public async Task<OpenCoursesDetailsViewModel?> GetCourseDetailsAsync(int id)
        {
            OpenCourse? openCourse = await courseRepository.GetOpenCourseByIdAsync(id);

            return openCourse == null ? null : new OpenCoursesDetailsViewModel
            {
                Title = openCourse.Title,
                Description = openCourse.Description,
                StartDate = openCourse.StartDate,
                ImageUrl = openCourse.ImageUrl,
                CategoryName = openCourse.Category?.Name ?? "General",
                AuthorEmail = openCourse.Creator?.Email ?? "Unknown"
            };
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

            bool valid = await courseRepository.AddOpenCourseAsync(openCourse);

            if (!valid)
            {
                throw new OpenCourseCreationFaliureExcpetion();
            }
        }

        public async Task<OpenCoursesCreateInputModel> CreateCourseInput()
        {
            IEnumerable<CourseCategory> categories = await courseRepository.GetAllCategories();

            return new OpenCoursesCreateInputModel
            {
                Categories = categories
            };
        }

        public async Task<OpenCoursesCreateInputModel?> GetCourseForEditAsync(int id, string userId)
        {
            OpenCourse? openCourse = await courseRepository.GetCourseByIdForEditAsync(id);

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
                Categories = await courseRepository.GetAllCategories()
            };

            return inputModel;
        }

        public async Task<bool> EditCourseAsync(int id, OpenCoursesCreateInputModel inputModel, string userId)
        {
            OpenCourse ?openCourse = await courseRepository.GetCourseForUpdateAsync(id);

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
                await courseRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<OpenCoursesDeleteViewModel?> GetCourseForDeleteAsync(int id, string userId)
        {
            OpenCourse? openCourse = await courseRepository.GetCourseByIdForEditAsync(id);

            if (openCourse == null)
            {
                return null;
            }

            if (userId == null || openCourse.CreatorId != userId)
            {
                return null;
            }

            OpenCoursesDeleteViewModel viewModel = new OpenCoursesDeleteViewModel
            {
                Id = openCourse.Id,
                Title = openCourse.Title
            };

            return viewModel;
        }

        public async Task<bool> DeleteCourseAsync(int id, string userId)
        {
            OpenCourse? course = await courseRepository.GetCourseForDeleteAsync(id);

            if (course == null || course.CreatorId != userId)
            {
                return false;
            }

            return await courseRepository.DeleteCourseAsync(course);
        }

        public async Task<Student?> GetStudentByIdAsync(string userId)
        {
            return await studentRepository.GetStudentByIdAsync(userId);
        }

        public async Task<OpenCourse?> GetCourseByIdAsync(int id)
        {
            return await courseRepository.GetCourseByIdAsync(id);
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

        public async Task UnenrollStudentAsync(Student student, OpenCourse course)
        {
            if (student.EnrolledCourses.Any(c => c.Id == course.Id))
            {
                student.EnrolledCourses.Remove(course);
                await courseRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CourseCategory>> GetCategoriesAsync()
        {
            return await courseRepository.GetAllCategories();
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            if (categoryId <= 0)
            {
                return false;
            }

            return await courseRepository.ExistsCategoryAsync(categoryId);
        }
    }
}
