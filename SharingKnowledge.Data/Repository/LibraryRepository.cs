using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data.Models;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Web.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Data.Repository
{
    public class LibraryRepository : ILibraryRepository
    {
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
                .OrderByDescending(b => b.Id)
                .ToListAsync();
        }
    
    }
}
