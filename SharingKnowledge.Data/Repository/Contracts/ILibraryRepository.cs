using SharingKnowledge.Data.Models;
using SharingKnowledge.Web.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Data.Repository.Contracts
{
    public interface ILibraryRepository
    {
        public Task<IEnumerable<Book>> GetAllBooksAsync();

        public Task AddAsync(Book book);

        public Task<int> SaveChangesAsync();

        public Task<Book?> GetBookByIdAsync(int id);

        public Task DeleteBookAsync(int id);
    }
}
