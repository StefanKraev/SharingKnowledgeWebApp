using SharingKnowledge.Data.Models;
using SharingKnowledge.Data.Repository;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.ViewModels.Courses;
using SharingKnowledge.Web.ViewModels.Books;
using SharingKnowledge.Web.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Services.Core
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryRepository libraryRepository;

        private readonly ICourseRepository courseRepository;

        private readonly IStudentRepository studentRepository;

        public LibraryService(ILibraryRepository libraryRepository, ICourseRepository courseRepository, IStudentRepository studentRepository)
        {
            this.libraryRepository = libraryRepository;
            this.courseRepository = courseRepository;
            this.studentRepository = studentRepository;
        }

        public async Task<IEnumerable<AllBooksViewModel>> GetAllBooksAsync(string? userId)
        {
            IEnumerable<Book> books = await libraryRepository.GetAllBooksAsync();

            int? studentIntId = null;

            if (userId != null)
            {
                studentIntId = await studentRepository.GetStudentIdByIdAsync(userId);
            }

            return books.Select(b => new AllBooksViewModel
            {
                Id = b.Id,
                Title = b.Title,
                AuthorName = b.AuthorName,
                ImageUrl = b.ImageUrl,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.Name,
                IsInUserLibrary = studentIntId.HasValue &&
                                  b.BookStudents.Any(bs => bs.StudentId == studentIntId.Value)
            }).ToList();
        }

        public async Task<BookCreateInputModel> CreateBookInput()
        {
            IEnumerable<CourseCategory> categories = await courseRepository.GetAllCategories();

            return new BookCreateInputModel
            {
                Categories = categories
            };
        }

        public async Task CreateBookAsync(BookCreateInputModel inputModel, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            Book newBook = new Book()
            {
                Title = inputModel.Title,
                AuthorName = inputModel.AuthorName,
                Description = inputModel.Description,
                ImageUrl = inputModel.ImageUrl,
                CategoryId = inputModel.CategoryId,
                PublisherId = userId
            };

            await libraryRepository.AddAsync(newBook);
            await libraryRepository.SaveChangesAsync();
        }

        public async Task<EditBookInputModel?> GetBookForEditAsync(int id, string userId)
        {
            var book = await libraryRepository.GetBookByIdAsync(id);

            if (book == null || book.PublisherId != userId)
            {
                return null;
            }

            return new EditBookInputModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorName = book.AuthorName,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                CategoryId = book.CategoryId,
                Categories = await courseRepository.GetAllCategories()
            };
        }

        public async Task EditBookAsync(EditBookInputModel model)
        {
            var book = await libraryRepository.GetBookByIdAsync(model.Id);

            if (book != null)
            {
                book.Title = model.Title;
                book.AuthorName = model.AuthorName;
                book.Description = model.Description;
                book.ImageUrl = model.ImageUrl;
                book.CategoryId = model.CategoryId;

                await libraryRepository.SaveChangesAsync();
            }
        }
        public async Task<DetailsBookViewModel?> GetBookForDeleteAsync(int id)
        {
            var book = await libraryRepository.GetBookByIdAsync(id);

            if (book == null)
            {
                return null;
            } 

            return new DetailsBookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorName = book.AuthorName,
                Description = book.Description,
                CategoryName = book.Category.Name
            };
        }

        public async Task<DetailsBookViewModel?> GetBookDetailsAsync(int id)
        {
            var book = await libraryRepository.GetBookByIdAsync(id);

            if (book == null)
            {
                return null;
            }

            return new DetailsBookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorName = book.AuthorName,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                CategoryName = book.Category.Name,
                PublisherId = book.PublisherId
            };
        }

        public async Task ExecuteDeleteAsync(int id)
        {
            await libraryRepository.DeleteBookAsync(id);
        }


    }
}
