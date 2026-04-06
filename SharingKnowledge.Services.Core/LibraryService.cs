using SharingKnowledge.Data.Models;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core.Interfaces;
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

        public LibraryService(ILibraryRepository libraryRepository)
        {
            this.libraryRepository = libraryRepository;
        }

        public async Task<IEnumerable<AllBooksViewModel>> GetAllBooksAsync()
        {
            IEnumerable<Book> books = await libraryRepository.GetAllBooksAsync();

            return books.Select(b => new AllBooksViewModel
            {
                Id = b.Id,
                Title = b.Title,
                AuthorName = b.AuthorName,
                ImageUrl = b.ImageUrl,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.Name
            }).ToList();
        }
    }
}
