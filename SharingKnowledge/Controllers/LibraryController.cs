using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharingKnowledge.Services.Core;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.Web.ViewModels.Books;
using System.Security.Claims;

namespace SharingKnowledge.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService libraryService;

        private readonly IOpenCoursesService openCoursesService;

        public LibraryController(ILibraryService libraryService, IOpenCoursesService openCoursesService)
        {
            this.libraryService = libraryService;
            this.openCoursesService = openCoursesService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            string? userId = GetUserId();

            IEnumerable<AllBooksViewModel> coursesAllViewModels = await libraryService.GetAllBooksAsync(userId);

            return View(coursesAllViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            string? userId = GetUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            EditBookInputModel model = await libraryService.GetBookForEditAsync(id, userId);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBookInputModel model)
        {
            var book = await libraryService.GetBookDetailsAsync(model.Id);

            if (book == null)
            {
                return NotFound();
            }

            if (book.PublisherId != GetUserId())
            {
                return Forbid();
            }

            var categories = await openCoursesService.GetCategoriesAsync();

            if (!categories.Any(c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Invalid Category.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = categories;
                return View(model);
            }

            await libraryService.EditBookAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BookCreateInputModel model = await libraryService.CreateBookInput();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateInputModel model)
        {
            string userId = GetUserId();

            if(userId == null)
            {
                return Unauthorized();
            }

            bool categoryExists = await openCoursesService.CategoryExistsAsync(model.CategoryId);

            if (!ModelState.IsValid || !categoryExists)
            {
                model.Categories = await openCoursesService.GetCategoriesAsync();
                return View(model);
            }

            await libraryService.CreateBookAsync(model, userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            DetailsBookViewModel model = await libraryService.GetBookForDeleteAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            DetailsBookViewModel book = await libraryService.GetBookDetailsAsync(id); // Fetch the raw entity

            if (book == null) return NotFound();

            if (book.PublisherId != GetUserId())
            {
                return Forbid();
            }

            await libraryService.ExecuteDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await libraryService.GetBookDetailsAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
    }
}
