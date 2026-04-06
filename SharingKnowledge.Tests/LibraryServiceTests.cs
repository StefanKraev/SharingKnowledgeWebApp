using Moq;
using SharingKnowledge.Data.Models;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core;
using SharingKnowledge.Web.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SharingKnowledge.Tests
{
    public class LibraryServiceTests
    {
        private readonly Mock<ILibraryRepository> repoMock;
        private readonly Mock<ICourseRepository> courseMock;
        private readonly Mock<IStudentRepository> studentMock;
        private readonly LibraryService service;

        public LibraryServiceTests()
        {
            repoMock = new Mock<ILibraryRepository>();
            courseMock = new Mock<ICourseRepository>();
            studentMock = new Mock<IStudentRepository>();

            service = new LibraryService(
                repoMock.Object,
                courseMock.Object,
                studentMock.Object);
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturnBooks()
        {
            // Arrange
            var userId = "user123";
            var studentIntId = 10;
            var books = new List<Book>
            {
                new Book {
                    Id = 1, Title = "Book 1",
                    Category = new CourseCategory { Name = "IT" },
                    BookStudents = new List<StudentBook> { new StudentBook { StudentId = 10 } }
                },
                new Book {
                    Id = 2, Title = "Book 2",
                    Category = new CourseCategory { Name = "Math" },
                    BookStudents = new List<StudentBook>()
                }
            };

            repoMock.Setup(r => r.GetAllBooksAsync()).ReturnsAsync(books);
            studentMock.Setup(s => s.GetStudentIdByIdAsync(userId)).ReturnsAsync(studentIntId);

            // Act
            var result = (await service.GetAllBooksAsync(userId)).ToList();

            // Assert
            Xunit.Assert.Equal(2, result.Count);
            Xunit.Assert.True(result[0].IsInUserLibrary); 
            Xunit.Assert.False(result[1].IsInUserLibrary);
        }

        [Fact]
        public async Task GetBookForEditAsync_ShouldReturnNull_WhenUserIsNotPublisher()
        {
            // Arrange
            var bookId = 1;
            var wrongUserId = "wrong-user";
            var book = new Book { Id = bookId, PublisherId = "real-publisher" };

            repoMock.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(book);

            // Act
            var result = await service.GetBookForEditAsync(bookId, wrongUserId);

            // Assert
            Xunit.Assert.Null(result);
        }

        [Fact]
        public async Task CreateBookAsync_ShouldCallAddAndSave_Successfully()
        {
            // Arrange
            var userId = "user1";
            var model = new BookCreateInputModel
            {
                Title = "Test",
                AuthorName = "Author",
                CategoryId = 1
            };

            // Act
            await service.CreateBookAsync(model, userId);

            // Assert
            repoMock.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task EditBookAsync_ShouldUpdateProperties_WhenBookExists()
        {
            // Arrange
            var existingBook = new Book { Id = 1, Title = "Old Title" };
            var model = new EditBookInputModel { Id = 1, Title = "New Title" };

            repoMock.Setup(r => r.GetBookByIdAsync(1)).ReturnsAsync(existingBook);

            // Act
            await service.EditBookAsync(model);

            // Assert
            Xunit.Assert.Equal("New Title", existingBook.Title);
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetBookDetailsAsync_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Arrange
            repoMock.Setup(r => r.GetBookByIdAsync(It.IsAny<int>())).ReturnsAsync((Book)null!);

            // Act
            var result = await service.GetBookDetailsAsync(999);

            // Assert
            Xunit.Assert.Null(result);
        }
    }
}
