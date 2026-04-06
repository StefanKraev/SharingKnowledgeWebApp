using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharingKnowledge.Services.Core;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.Web.ViewModels.Books;

namespace SharingKnowledge.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            this.libraryService = libraryService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            string? userId = GetUserId();

            IEnumerable<AllBooksViewModel> coursesAllViewModels = await libraryService.GetAllBooksAsync();

            return View(coursesAllViewModels);
        }
    }
}
