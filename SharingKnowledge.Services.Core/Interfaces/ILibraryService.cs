using SharingKnowledge.Web.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Services.Core.Interfaces
{
    public interface ILibraryService
    {
        public Task<IEnumerable<AllBooksViewModel>> GetAllBooksAsync();


    }
}
