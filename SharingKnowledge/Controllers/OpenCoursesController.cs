using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SharingKnowledge.Data;
using SharingKnowledge.Models;
using SharingKnowledge.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Authorization;
using SharingKnowledge.Services.Core.Interfaces;

namespace SharingKnowledge.Controllers
{
    public class OpenCoursesController : ControllerBase
    {
        private readonly ApplicationDbContext DbContext;

        private readonly IOpenCoursesService openCoursesService;

        public OpenCoursesController(ApplicationDbContext dbContext, IOpenCoursesService openCoursesService)
        {
            this.DbContext = dbContext;
            this.openCoursesService = openCoursesService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<OpenCoursesAllViewModel> coursesAllViewModels =
                await openCoursesService.GetAllCoursesAsync();

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
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                ModelState.AddModelError(string.Empty, "An error occurred while creating the open course. Please try again.");
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
                    (await openCoursesService.GetCategoriesAsync()).ToList();

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

            OpenCourse? openCourse = await DbContext
                .OpenCourses
                .AsNoTracking()
                .SingleOrDefaultAsync(oc => oc.Id == id);

            if (openCourse == null)
            {
                return NotFound();
            }

            if (userId == null || openCourse.CreatorId != userId)
            {
                return Forbid();
            }

            OpenCoursesDeleteViewModel viewModel = new OpenCoursesDeleteViewModel
            {
                Id = openCourse.Id,
                Title = openCourse.Title
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, OpenCoursesDeleteViewModel viewModel)
        {
            string? userId = GetUserId();

            if (id <= 0)
            {
                return BadRequest();
            }

            OpenCourse? openCourse = await DbContext
                .OpenCourses
                .SingleOrDefaultAsync(oc => oc.Id == id);

            if (openCourse == null)
            {
                return NotFound();
            }

            if (userId == null || openCourse.CreatorId != userId)
            {
                return Forbid();
            }

            try
            {
                DbContext.OpenCourses.Remove(openCourse);
                await DbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the open course. Please try again.");
                return View(viewModel);
            }
        }

    }
}
