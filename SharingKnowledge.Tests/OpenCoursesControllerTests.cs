using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SharingKnowledge.Controllers;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.ViewModels.Courses;
using SharingKnowledge.Web.ViewModels.Courses;
using System.Security.Claims;
using Xunit;

namespace SharingKnowledge.Tests
{
    public class OpenCoursesControllerTests
    {
        private readonly Mock<IOpenCoursesService> mockCourseService;
        private readonly Mock<ILogger<OpenCoursesController>> mockLogger;
        private readonly OpenCoursesController openCoursesController;
        private const string TestUserId = "user-123";

        public OpenCoursesControllerTests()
        {
            mockCourseService = new Mock<IOpenCoursesService>();
            mockLogger = new Mock<ILogger<OpenCoursesController>>();

            openCoursesController = new OpenCoursesController(mockCourseService.Object, mockLogger.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, TestUserId),
            }, "mock"));

            openCoursesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task IndexPage_ReturnsViewWithModels_Successfully()
        {
            // Arrange
            var courses = new List<MyCoursesViewModel>();
            mockCourseService.Setup(s => s.GetAllCoursesAsync(TestUserId, null, null, 1, 6))
                .ReturnsAsync((courses, 5));

            // Act
            var result = await openCoursesController.Index(null, null);

            // Assert
            var viewResult = Xunit.Assert.IsType<ViewResult>(result);
            Xunit.Assert.Equal(courses, viewResult.Model);
            Xunit.Assert.Equal(5, openCoursesController.ViewData["TotalPages"]);
        }

        [Fact]
        public async Task Details_InvalidId_ReturnsBadRequest()
        {
            // Act
            var result = await openCoursesController.Details(0);

            // Assert
            Xunit.Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Details_CourseNotFound_ReturnsNotFound()
        {
            // Arrange
            mockCourseService.Setup(s => s.GetCourseDetailsAsync(99))
                .ReturnsAsync((OpenCoursesDetailsViewModel?)null);

            // Act
            var result = await openCoursesController.Details(99);

            // Assert
            Xunit.Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_InvalidModelState_ReturnsViewWithCategories()
        {
            // Arrange
            openCoursesController.ModelState.AddModelError("Title", "Required");
            var inputModel = new OpenCoursesCreateInputModel();
            mockCourseService.Setup(s => s.GetCategoriesAsync())
                .ReturnsAsync(new List<CourseCategory>());

            // Act
            var result = await openCoursesController.Create(inputModel);

            // Assert
            var viewResult = Xunit.Assert.IsType<ViewResult>(result);
            mockCourseService.Verify(s => s.GetCategoriesAsync(), Times.Once);
        }

        [Fact]
        public async Task Create_ValidData_RedirectsToIndex()
        {
            // Arrange
            var inputModel = new OpenCoursesCreateInputModel { CategoryId = 1 };
            mockCourseService.Setup(s => s.CategoryExistsAsync(1)).ReturnsAsync(true);

            // Act
            var result = await openCoursesController.Create(inputModel);

            // Assert
            var redirect = Xunit.Assert.IsType<RedirectToActionResult>(result);
            Xunit.Assert.Equal("Index", redirect.ActionName);
            mockCourseService.Verify(s => s.CreateCourseAsync(inputModel, TestUserId), Times.Once);
        }

        [Fact]
        public async Task Edit_OwnerAccess_ReturnsView()
        {
            // Arrange
            var inputModel = new OpenCoursesCreateInputModel();
            mockCourseService.Setup(s => s.GetCourseForEditAsync(1, TestUserId))
                .ReturnsAsync(inputModel);

            // Act
            var result = await openCoursesController.Edit(1);

            // Assert
            var viewResult = Xunit.Assert.IsType<ViewResult>(result);
            Xunit.Assert.Equal(inputModel, viewResult.Model);
        }

        [Fact]
        public async Task Delete_Success_RedirectsToIndex()
        {
            // Arrange
            mockCourseService.Setup(s => s.DeleteCourseAsync(1, TestUserId))
                .ReturnsAsync(true);

            // Act
            var result = await openCoursesController.Delete(1, new OpenCoursesDeleteViewModel());

            // Assert
            var redirect = Xunit.Assert.IsType<RedirectToActionResult>(result);
            Xunit.Assert.Equal("Index", redirect.ActionName);
        }
    }
}
