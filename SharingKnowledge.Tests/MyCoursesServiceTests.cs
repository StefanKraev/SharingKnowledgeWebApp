using Microsoft.EntityFrameworkCore;
using Moq;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SharingKnowledge.Tests
{
    public class MyCoursesServiceTests
    {
        private readonly Mock<ICourseRepository> mockCourseRepo;
        private readonly Mock<IStudentRepository> mockStudentRepo;
        private readonly MyCoursesService service;

        public MyCoursesServiceTests()
        {
            mockCourseRepo = new Mock<ICourseRepository>();
            mockStudentRepo = new Mock<IStudentRepository>();
            service = new MyCoursesService(mockCourseRepo.Object, mockStudentRepo.Object);
        }

        [Fact]
        public async Task AddCourseToStudentAsync_PositivePath_ShouldAddCourseToCollectionAndSave()
        {
            // Arrange
            var course = new OpenCourse { Id = 1, Title = "Unit Testing 101" };
            var student = new Student
            {
                Id = 10,
                EnrolledCourses = new List<OpenCourse>()
            };

            mockCourseRepo.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(true);

            // Act
            await service.AddCourseToStudentAsync(course, student);

            // Assert
            Xunit.Assert.Contains(course, student.EnrolledCourses);
            Xunit.Assert.Single(student.EnrolledCourses);

            mockCourseRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddCourseToStudentAsync_NegativePath_ShouldThrowException()
        {
            // Arrange
            var course = new OpenCourse { Id = 1 };
            var student = new Student { EnrolledCourses = new List<OpenCourse>() };

            mockCourseRepo.Setup(r => r.SaveChangesAsync())
                .ThrowsAsync(new DbUpdateException("Database error occurred"));

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<DbUpdateException>(() =>
                service.AddCourseToStudentAsync(course, student));
        }

        [Fact]
        public async Task GetAllStudentCourses_WhenNoErrorsOccure_ShouldMapAllProperties()
        {
            // Arrange
            var student = new Student
            {
                Id = 1,
                EnrolledCourses = new List<OpenCourse>
                {
                    new OpenCourse
                    {
                        Id = 10,
                        Title = "Test Course",
                        Description = "Description",
                        StartDate = DateTime.Now,
                        ImageUrl = "test.jpg",
                        CreatorId = 5,
                        Category = new CourseCategory { Name = "Programming" }
                    }
                }
            };

            // Act
            var result = await service.GetAllStudentCourses(student);

            // Assert
            var course = Xunit.Assert.Single(result);
            Xunit.Assert.Equal(10, course.Id);
            Xunit.Assert.Equal("Test Course", course.Title);
            Xunit.Assert.Equal("Programming", course.CategoryName);
            Xunit.Assert.True(course.IsEnrolled);
        }

        [Fact]
        public async Task GetAllStudentCourses_WhenCategoryIsNull_ShouldReturnGeneral()
        {
            // Arrange
            var student = new Student
            {
                EnrolledCourses = new List<OpenCourse>
                {
                    new OpenCourse { Title = "No Category Course", Category = null! }
                }
            };

            // Act
            var result = await service.GetAllStudentCourses(student);

            // Assert
            var course = Xunit.Assert.Single(result);
            Xunit.Assert.Equal("General", course.CategoryName);
        }

        [Fact]
        public async Task GetAllStudentCourses_WhenEnrolledCoursesIsEmpty_ShouldReturnEmptyCollection()
        {
            // Arrange
            var student = new Student { EnrolledCourses = new List<OpenCourse>() };

            // Act
            var result = await service.GetAllStudentCourses(student);

            // Assert
            Xunit.Assert.Empty(result);
        }

        [Fact]
        public async Task GetCourseByIdAsync_WhenCourseExists_ShouldReturnCourse()
        {
            // Arrange
            int testId = 42;
            var expectedCourse = new OpenCourse { Id = testId, Title = "Unit Testing Pro" };

            mockCourseRepo.Setup(r => r.GetCourseByIdAsync(testId))
                .ReturnsAsync(expectedCourse);

            // Act
            var result = await service.GetCourseByIdAsync(testId);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(testId, result.Id);
            Xunit.Assert.Equal("Unit Testing Pro", result.Title);
            mockCourseRepo.Verify(r => r.GetCourseByIdAsync(testId), Times.Once);
        }

        [Fact]
        public async Task GetCourseByIdAsync_WhenCourseDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            int missingId = 999;
            mockCourseRepo.Setup(r => r.GetCourseByIdAsync(missingId))
                .ReturnsAsync((OpenCourse?)null);

            // Act
            var result = await service.GetCourseByIdAsync(missingId);

            // Assert
            Xunit.Assert.Null(result);
            mockCourseRepo.Verify(r => r.GetCourseByIdAsync(missingId), Times.Once);
        }

        [Fact]
        public async Task GetStudentByIdAsync_WhenUserExists_ReturnsStudent()
        {
            // Arrange
            string userId = "guid-123-abc";
            var expectedStudent = new Student
            {
                Id = 1,
                UserId = userId,
                EnrolledCourses = new List<OpenCourse>()
            };

            mockStudentRepo.Setup(r => r.GetStudentByIdAsync(userId))
                .ReturnsAsync(expectedStudent);

            // Act
            var result = await service.GetStudentByIdAsync(userId);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(userId, result.UserId);

            mockStudentRepo.Verify(r => r.GetStudentByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetStudentByIdAsync_WhenUserDoesNotExist_ReturnsNull()
        {
            // Arrange
            string unknownUserId = "non-existent-id";
            mockStudentRepo.Setup(r => r.GetStudentByIdAsync(unknownUserId))
                .ReturnsAsync((Student?)null);

            // Act
            var result = await service.GetStudentByIdAsync(unknownUserId);

            // Assert
            Xunit.Assert.Null(result);
            mockStudentRepo.Verify(r => r.GetStudentByIdAsync(unknownUserId), Times.Once);
        }

        [Fact]
        public async Task UnenrollStudentAsync_WhenEnrolled_ShouldRemoveAndSave()
        {
            // Arrange
            var course = new OpenCourse { Id = 1, Title = "Test Course" };
            var student = new Student
            {
                Id = 10,
                EnrolledCourses = new List<OpenCourse> { course }
            };

            mockCourseRepo.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(true);

            // Act
            await service.UnenrollStudentAsync(student, course);

            // Assert
            Xunit.Assert.DoesNotContain(course, student.EnrolledCourses);
            Xunit.Assert.Empty(student.EnrolledCourses);
            mockCourseRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UnenrollStudentAsync_WhenNotEnrolled_ShouldDoNothing()
        {
            // Arrange
            var courseToUnenroll = new OpenCourse { Id = 1 };
            var differentCourse = new OpenCourse { Id = 2 };

            var student = new Student
            {
                EnrolledCourses = new List<OpenCourse> { differentCourse }
            };

            // Act
            await service.UnenrollStudentAsync(student, courseToUnenroll);

            // Assert
            Xunit.Assert.Contains(differentCourse, student.EnrolledCourses);
            mockCourseRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UnenrollStudentAsync_WhenCollectionIsEmpty_ShouldNotCrash()
        {
            // Arrange
            var course = new OpenCourse { Id = 1 };
            var student = new Student { EnrolledCourses = new List<OpenCourse>() };

            // Act
            await service.UnenrollStudentAsync(student, course);

            // Assert
            Xunit.Assert.Empty(student.EnrolledCourses);
            mockCourseRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
