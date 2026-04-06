using Moq;
using SharingKnowledge.Data.Models;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Common.Exceptions;
using SharingKnowledge.Services.Core;
using SharingKnowledge.ViewModels.Courses;
using Xunit;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace SharingKnowledge.Tests
{
    public class OpenCourseServiceTests
    {
        private readonly Mock<ICourseRepository> mockCourseRepo;
        private readonly Mock<IStudentRepository> mockStudentRepo;
        private readonly OpenCoursesService service;

        public OpenCourseServiceTests()
        {
            this.mockCourseRepo = new Mock<ICourseRepository>();
            this.mockStudentRepo = new Mock<IStudentRepository>();
            this.service = new OpenCoursesService(mockCourseRepo.Object, mockStudentRepo.Object);
        }

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

            mockCourseRepo.Setup(repo => repo.GetOpenCourseByIdAsync(1))
                .ReturnsAsync(openCourse);
       

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
            mockCourseRepo.Setup(repo => repo.GetOpenCourseByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((OpenCourse?)null);

            //Act
            var result = await service.GetCourseDetailsAsync(99999);

            //Assert
            Xunit.Assert.Null(result);
        }

        [Fact]
        public async Task CreateCourseAsync_WhenStudentExists_ShouldCallAddCourse()
        {
            // Arrange
            var userId = "user-123";
            var student = new Student { Id = 10, UserId = userId };
            var inputModel = new OpenCoursesCreateInputModel
            {
                Title = "New Course",
                Description = "Description",
                StartDate = DateTime.Now,
                CategoryId = 1
            };

            mockStudentRepo.Setup(r => r.GetStudentByIdAsync(userId))
                .ReturnsAsync(student);

            mockCourseRepo.Setup(r => r.AddOpenCourseAsync(It.IsAny<OpenCourse>()))
                .ReturnsAsync(true); // As we are testing only the existence of the user

            // Act
            await service.CreateCourseAsync(inputModel, userId);

            // Assert
            mockCourseRepo.Verify(r => r.AddOpenCourseAsync(It.Is<OpenCourse>(c =>
                c.Title == inputModel.Title &&
                c.CreatorId == student.Id)), Times.Once);
        }

        [Fact]
        public async Task CreateCourseAsync_WhenStudentDoesNotExists_ShouldCallAddCourse()
        {
            // Arrange

            mockStudentRepo.Setup(r => r.GetStudentByIdAsync("dummyId"))
                .ReturnsAsync((Student?)null);

            var inputModel = new OpenCoursesCreateInputModel();

            // Assert/Act
            await Xunit.Assert.ThrowsAsync<Exception>(() =>
             service.CreateCourseAsync(inputModel, "dummyId"));
        }

        [Fact]
        public async Task CreateCourseAsync_WhenRepositoryFails_ShouldThrowSpecificException()
        {
            // Arrange
            var userId = "user-123";
            var student = new Student { Id = 10, UserId = userId }; //Valid Student
            var inputModel = new OpenCoursesCreateInputModel
            {
                Title = "New Course",
                Description = "Description",
                StartDate = DateTime.Now,
                CategoryId = 1
            };

            mockStudentRepo.Setup(r => r.GetStudentByIdAsync(userId))
                .ReturnsAsync(student);

            mockCourseRepo.Setup(r => r.AddOpenCourseAsync(It.IsAny<OpenCourse>()))
                .ReturnsAsync(false);

            // Act/Assert
            await Xunit.Assert.ThrowsAsync<OpenCourseCreationFaliureExcpetion>(() =>
             service.CreateCourseAsync(inputModel, userId));
        }

        [Fact]
        public async Task CreateCourseInput_ShouldReturnModelWithCategoris()
        {
            // Arrange
            var categoriesList = new List<CourseCategory>
            {
                new CourseCategory { Id = 1, Name = "Development" },
                new CourseCategory { Id = 2, Name = "Business" }
            };

            mockCourseRepo.Setup(r => r.GetAllCategories())
                .ReturnsAsync(categoriesList);

            // Act
            var result = await service.CreateCourseInput();

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.NotNull(result.Categories);
            Xunit.Assert.Equal(2, result.Categories.Count());
            Xunit.Assert.Contains(result.Categories, c => c.Name == "Development");
        }

        [Fact]
        public async Task CreateCourseInput_ShouldReturnEmptyList_WhenNoCategorysExist()
        {
            // Arrange
            mockCourseRepo.Setup(r => r.GetAllCategories())
                .ReturnsAsync(new List<CourseCategory>());

            // Act
            var result = await service.CreateCourseInput();

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Empty(result.Categories);
        }

        [Fact]
        public async Task GetCourseForEditAsync_WhenOwnerRequests_ReturnsMappedCourse()
        {
            // Arrange
            var userId = "owner-id";
            var openCourse = new OpenCourse
            {
                Id = 1,
                Title = "Original Title",
                Creator = new Student { UserId = userId },
                CategoryId = 5
            };
            var categories = new List<CourseCategory> { new CourseCategory { Id = 5, Name = "IT" } };

            mockCourseRepo.Setup(r => r.GetCourseByIdForEditAsync(1))
                .ReturnsAsync(openCourse);
            mockCourseRepo.Setup(r => r.GetAllCategories())
                .ReturnsAsync(categories);

            // Act
            var result = await service.GetCourseForEditAsync(1, userId);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal("Original Title", result.Title);
            Xunit.Assert.Equal(5, result.CategoryId);
            Xunit.Assert.NotEmpty(result.Categories);
        }

        [Fact]
        public async Task GetCourseForEditAsync_WrongUserRequests_ReturnsNull()
        {
            // Arrange
            var ownerId = "actual-owner";
            var hackerId = "attacker-id";

            var openCourse = new OpenCourse
            {
                Id = 1,
                Creator = new Student { UserId = ownerId }
            };

            mockCourseRepo.Setup(r => r.GetCourseByIdForEditAsync(1))
                .ReturnsAsync(openCourse);

            // Act
            var result = await service.GetCourseForEditAsync(1, hackerId);

            // Assert
            Xunit.Assert.Null(result);
        }

        [Fact]
        public async Task GetCourseForEditAsync_WhenCourseDoesNotExist_ReturnsNull()
        {
            // Arrange
            mockCourseRepo.Setup(r => r.GetCourseByIdForEditAsync(It.IsAny<int>()))
                .ReturnsAsync((OpenCourse?)null);

            // Act
            var result = await service.GetCourseForEditAsync(999, "any-user");

            // Assert
            Xunit.Assert.Null(result);
        }
    }
}
