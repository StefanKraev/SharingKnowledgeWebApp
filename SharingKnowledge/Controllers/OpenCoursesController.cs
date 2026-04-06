using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SharingKnowledge.Data;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.ViewModels.Courses;
using SharingKnowledge.Web.ViewModels.Courses;
using System.Security.Claims;
using SharingKnowledge.Services.Common;
using SharingKnowledge.Services.Common.Exceptions;
using static SharingKnowledge.Common.OutputMessages.OpenCourseMessages;
using static SharingKnowledge.Common.OutputMessages.GenericMessages;

namespace SharingKnowledge.Controllers
{
    public class OpenCoursesController : ControllerBase
    {
        private readonly ILogger<OpenCoursesController> logger;

        private readonly IOpenCoursesService openCoursesService;

        public OpenCoursesController(IOpenCoursesService openCoursesService, ILogger<OpenCoursesController> logger)
        {
            this.openCoursesService = openCoursesService;
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? searchQuery, string? category, int pageNumber = 1, int pageSize = 6)
        {
            string? userId = GetUserId();

            var (coursesAllViewModels, totalPages) =
                await openCoursesService.GetAllCoursesAsync(userId, searchQuery, category, pageNumber, pageSize);

            ViewData["CurrentSearchQuery"] = searchQuery;
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;

            return View(coursesAllViewModels);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if(id <= 0)
            {
                return BadRequest();
            }

            OpenCoursesDetailsViewModel? openCourse = 
                await openCoursesService.GetCourseDetailsAsync(id);

            if (openCourse == null)
            {
                return NotFound();
            }

            return View(openCourse);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            OpenCoursesCreateInputModel openCoursesCreateInputModel = 
                await openCoursesService.CreateCourseInput();

            return View(openCoursesCreateInputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OpenCoursesCreateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                inputModel.Categories = (await openCoursesService.GetCategoriesAsync()).ToList();
                return View(inputModel);
            }

            if (await openCoursesService.CategoryExistsAsync(inputModel.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(inputModel.CategoryId), "Selected category does not exist.");
                inputModel.Categories = (await openCoursesService.GetCategoriesAsync()).ToList();
                return View(inputModel);
            }

            string? userId = GetUserId();

            if(string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                await openCoursesService.CreateCourseAsync(inputModel, userId);

                return RedirectToAction(nameof(Index));
            }
            catch (OpenCourseCreationFaliureExcpetion exception)
            {
                logger.LogError(exception, OpenCourseCreationFailureMessage);

                Console.WriteLine(exception);
                ModelState.AddModelError(string.Empty, OpenCourseCreationFailureMessage);
                inputModel.Categories = (await openCoursesService.GetCategoriesAsync()).ToList();
                return View(inputModel);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, UnexpectedErrorMessage);

                Console.WriteLine(exception);
                ModelState.AddModelError(string.Empty, UnexpectedErrorMessage);
                inputModel.Categories = (await openCoursesService.GetCategoriesAsync()).ToList();
                return View(inputModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
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

            OpenCoursesCreateInputModel ?inputModel = 
                await openCoursesService.GetCourseForEditAsync(id, userId);

            if (inputModel == null)
            {
                return NotFound();
            }

            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, OpenCoursesCreateInputModel inputModel)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            string? userId = GetUserId();

            if (userId == null) 
            {
                return RedirectToAction("Login", "Account"); ;
            }

            bool categoryExists = 
                await openCoursesService.CategoryExistsAsync(inputModel.CategoryId);

            if (!ModelState.IsValid || !categoryExists)
            {
                if (!categoryExists)
                {
                    ModelState.AddModelError(nameof(inputModel.CategoryId), "Selected category does not exist.");
                }

                inputModel.Categories = 
                    (await openCoursesService.GetCategoriesAsync());

                return View(inputModel);
            }

            bool isSuccess = 
                await openCoursesService.EditCourseAsync(id, inputModel, userId);

            if (!isSuccess)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
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

            OpenCoursesDeleteViewModel? viewModel =
                await openCoursesService.GetCourseForDeleteAsync(id, userId);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, OpenCoursesDeleteViewModel viewModel)
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

            bool isDeleted = 
                await openCoursesService.DeleteCourseAsync(id, userId);

            if (!isDeleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
