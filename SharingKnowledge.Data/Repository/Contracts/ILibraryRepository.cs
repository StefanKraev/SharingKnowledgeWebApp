using SharingKnowledge.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Data.Repository.Contracts
{
    public interface ILibraryRepository
    {
        public Task<IEnumerable<Book>> GetAllBooksAsync();
    }
}
