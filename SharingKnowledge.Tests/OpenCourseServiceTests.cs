using Moq;
using SharingKnowledge.Data.Models;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core;
using SharingKnowledge.ViewModels.Courses;
using Xunit;

namespace SharingKnowledge.Tests
{
    public class OpenCourseServiceTests
    {
        [Fact]
        public async Task GetCourseDetailsAsync_whenCourseExists_ReturnsTrue()
        {
            //Arrange
            var openCourse = new OpenCourse
            {
                Id = 1,
                Title = "Test Course",
                Description = "This is a test course.",
                CategoryId = 1,
                StartDate = DateTime.Now,
                Category = new CourseCategory { Name = "Programming" },
                Creator = new Student
                {
                    User = new ApplicationUser { Email = "author@test.com" }
                }
            };

            var mockStudentRepository = new Mock<IStudentRepository>();

            var mockCourseRepository = new Mock<ICourseRepository>();
                
            mockCourseRepository.Setup(repo => repo.GetOpenCourseByIdAsync(1))
                .ReturnsAsync(openCourse);

            var service = new OpenCoursesService(mockCourseRepository.Object, mockStudentRepository.Object);

            //Act
            var result = await service.GetCourseDetailsAsync(1);

            //Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.IsType<OpenCoursesDetailsViewModel>(result);
            Xunit.Assert.Equal("Test Course", result.Title);
        }

        [Fact]
        public async Task GetCourseDetailsAsync_whenCourseDoesNotExists_ReturnsFalse()
        {
            //Arrange
            var mockStudentRepository = new Mock<IStudentRepository>();

            var mockCourseRepository = new Mock<ICourseRepository>();

            mockCourseRepository.Setup(repo => repo.GetOpenCourseByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((OpenCourse?)null);

            var service = new OpenCoursesService(mockCourseRepository.Object, mockStudentRepository.Object);

            //Act
            var result = await service.GetCourseDetailsAsync(99999);

            //Assert
            Xunit.Assert.Null(result);
        }
    }
}
