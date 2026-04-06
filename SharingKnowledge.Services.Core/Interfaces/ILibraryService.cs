using SharingKnowledge.Data.Models;
using SharingKnowledge.ViewModels.Courses;
using SharingKnowledge.Web.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Services.Core.Interfaces
{
    public interface ILibraryService
    {
        public Task<IEnumerable<AllBooksViewModel>> GetAllBooksAsync(string? userId);

        public Task<BookCreateInputModel> CreateBookInput();

        public Task CreateBookAsync(BookCreateInputModel inputModel, string userId);

        public Task<EditBookInputModel?> GetBookForEditAsync(int id, string userId);

        public Task EditBookAsync(EditBookInputModel model);

        public Task<DetailsBookViewModel?> GetBookForDeleteAsync(int id);

        public Task ExecuteDeleteAsync(int id);

        public Task<DetailsBookViewModel?> GetBookDetailsAsync(int id);
    }
}
