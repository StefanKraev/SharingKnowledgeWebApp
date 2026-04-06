using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data.Models;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Web.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Data.Repository
{
    public class LibraryRepository : ILibraryRepository, IDisposable
    {
        private bool isDisposed = false;

        private readonly ApplicationDbContext context;

        public LibraryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
             return await context
                .Books
                .AsNoTracking() 
                .Include(b => b.Category)
                .Include(c => c.BookStudents)
                .OrderByDescending(b => b.Id)
                .ToListAsync();
        }

        public async Task AddAsync(Book book)
        {
            await context.Books.AddAsync(book);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            Book book = await context.Books.FindAsync(id);
            if (book != null)
            {
                context.Books.Remove(book);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await context.Books
                .Include(b => b.Category) 
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                isDisposed = true;
            }
        }
    }
}
