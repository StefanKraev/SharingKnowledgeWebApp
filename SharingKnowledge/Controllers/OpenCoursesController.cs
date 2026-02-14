using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SharingKnowledge.Data;
using SharingKnowledge.Models;
using SharingKnowledge.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace SharingKnowledge.Controllers
{
    public class OpenCoursesController : ControllerBase
    {
        private readonly ApplicationDbContext DbContext;
        public OpenCoursesController(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<OpenCoursesAllViewModel> coursesAllViewModels = await DbContext
                .OpenCourses
                .AsNoTracking()
                .Select(oc => new OpenCoursesAllViewModel
                {
                    Id = oc.Id,
                    Title = oc.Title,
                    Description = oc.Description.
                            Length > 100
                          ? oc.Description.Substring(0, 97) + "..."
                          : oc.Description,
                    StartDate = oc.StartDate,
                    ImageUrl = oc.ImageUrl,
                    CategoryName = oc.Category.Name
                })
                .ToListAsync();

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

            OpenCoursesDetailsViewModel? openCourse = await DbContext
                .OpenCourses
                .AsNoTracking()
                .Where(oc => oc.Id == id)
                .Select(oc => new OpenCoursesDetailsViewModel
                {
                    Title = oc.Title,
                    Description = oc.Description,
                    StartDate = oc.StartDate,
                    ImageUrl = oc.ImageUrl,
                    CategoryName = oc.Category.Name,
                    AuthorEmail = oc.Creator.Email ?? "Email not found!"
                })
                .SingleOrDefaultAsync();

            if (openCourse == null)
            {
                return NotFound();
            }

            return View(openCourse);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            OpenCoursesCreateInputModel openCoursesCreateInputModel = new OpenCoursesCreateInputModel
            {
                Categories = await DbContext
                    .CourseCategories
                    .AsNoTracking()
                    .OrderBy(cc => cc.Name)
                    .ToListAsync()
            };

            return View(openCoursesCreateInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OpenCoursesCreateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesAsync(inputModel);
                return View(inputModel);
            }

            if (await CourseCategoryExistsAsync(inputModel.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(inputModel.CategoryId), "Selected category does not exist.");
                await PopulateCategoriesAsync(inputModel);
                return View(inputModel);
            }

            string? userId = GetUserId();

            if(string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                OpenCourse openCourse = new OpenCourse
                {
                    Title = inputModel.Title,
                    Description = inputModel.Description,
                    StartDate = inputModel.StartDate,
                    ImageUrl = inputModel.ImageUrl,
                    CategoryId = inputModel.CategoryId,
                    CreatorId = userId,
                    EnrolledStudents = new List<Student>()
                };

                await DbContext.OpenCourses.AddAsync(openCourse);
                await DbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                ModelState.AddModelError(string.Empty, "An error occurred while creating the open course. Please try again.");
                await PopulateCategoriesAsync(inputModel);
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

            OpenCourse? openCourse = await DbContext
                .OpenCourses
                .Include(oc => oc.Category)
                .SingleOrDefaultAsync(oc => oc.Id == id);

            if (openCourse == null)
            {
                return NotFound();
            }

            OpenCoursesCreateInputModel inputModel = new OpenCoursesCreateInputModel
            {
                Title = openCourse.Title,
                Description = openCourse.Description,
                StartDate = openCourse.StartDate,
                ImageUrl = openCourse.ImageUrl,
                CategoryId = openCourse.CategoryId,
                Categories = await DbContext
                    .CourseCategories
                    .AsNoTracking()
                    .OrderBy(cc => cc.Name)
                    .ToListAsync()
            };

            return View(inputModel);

        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int id, OpenCoursesCreateInputModel inputModel)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            
            OpenCourse? openCourse = await DbContext
                .OpenCourses
                .Include(oc => oc.Category)
                .SingleOrDefaultAsync(oc => oc.Id == id);

            if(openCourse == null)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                await PopulateCategoriesAsync(inputModel);
                return View(inputModel);
            }

            if(await CourseCategoryExistsAsync(inputModel.CategoryId) == false)
            {
                await PopulateCategoriesAsync(inputModel);
                ModelState.AddModelError(nameof(inputModel.CategoryId), "Selected category does not exist.");
                return View(inputModel);
            }

            try
            {
                openCourse.Title = inputModel.Title;
                openCourse.Description = inputModel.Description;
                openCourse.StartDate = inputModel.StartDate;
                openCourse.ImageUrl = inputModel.ImageUrl;
                openCourse.CategoryId = inputModel.CategoryId;

                await DbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception exception)
            {
                await PopulateCategoriesAsync(inputModel);
                Console.WriteLine(exception);
                ModelState.AddModelError(string.Empty, "An error occurred while editing the open course. Please try again.");
                return View(inputModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            OpenCourse? openCourse = await DbContext
                .OpenCourses
                .AsNoTracking()
                .SingleOrDefaultAsync(oc => oc.Id == id);

            if (openCourse == null)
            {
                return NotFound();
            }

            OpenCoursesDeleteViewModel viewModel = new OpenCoursesDeleteViewModel
            {
                Title = openCourse.Title
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, OpenCoursesDeleteViewModel viewModel)
        {
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

        private async Task PopulateCategoriesAsync(OpenCoursesCreateInputModel model)
        {
            model.Categories = await DbContext.CourseCategories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        private async Task<bool> CourseCategoryExistsAsync(int categoryId)
        {
            return await DbContext.CourseCategories.AnyAsync(c => c.Id == categoryId);
        }

    }
}
