using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SharingKnowledge.Data;
using SharingKnowledge.Models;
using SharingKnowledge.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Contracts;

namespace SharingKnowledge.Controllers
{
    public class OpenCoursesController : Controller
    {
        private readonly ApplicationDbContext DbContext;
        public OpenCoursesController(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<OpenCoursesAllViewModel> coursesAllViewModels = DbContext
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
                .ToList();

            return View(coursesAllViewModels);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if(id <= 0)
            {
                return BadRequest();
            }

            OpenCoursesDetailsViewModel? openCourse = DbContext
                .OpenCourses
                .AsNoTracking()
                .Where(oc => oc.Id == id)
                .Select(oc => new OpenCoursesDetailsViewModel
                {
                    Title = oc.Title,
                    Description = oc.Description,
                    StartDate = oc.StartDate,
                    ImageUrl = oc.ImageUrl,
                    CategoryName = oc.Category.Name
                })
                .SingleOrDefault();

            if (openCourse == null)
            {
                return NotFound();
            }

            return View(openCourse);
        }

        [HttpGet]
        public IActionResult Create()
        {
            OpenCoursesCreateInputModel openCoursesCreateInputModel = new OpenCoursesCreateInputModel
            {
                Categories = DbContext
                    .CourseCategories
                    .AsNoTracking()
                    .OrderBy(cc => cc.Name)
                    .ToList()
            };

            return View(openCoursesCreateInputModel);
        }

        [HttpPost]
        public IActionResult Create(OpenCoursesCreateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            if(CourseCategoryExists(inputModel.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(inputModel.CategoryId), "Selected category does not exist.");
                return View(inputModel);
            }

            try
            {
                OpenCourse openCourse = new OpenCourse
                {
                    Title = inputModel.Title,
                    Description = inputModel.Description,
                    StartDate = inputModel.StartDate,
                    ImageUrl = inputModel.ImageUrl,
                    CategoryId = inputModel.CategoryId
                };

                DbContext.OpenCourses.Add(openCourse);
                DbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                ModelState.AddModelError(string.Empty, "An error occurred while creating the open course. Please try again.");
                return View(inputModel);
            }
        }

        [HttpGet]
        public IActionResult Edit([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            OpenCourse? openCourse = DbContext
                .OpenCourses
                .Include(oc => oc.Category)
                .SingleOrDefault(oc => oc.Id == id);

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
                Categories = DbContext
                    .CourseCategories
                    .AsNoTracking()
                    .OrderBy(cc => cc.Name)
                    .ToList()
            };

            return View(inputModel);

        }

        [HttpPost]
        public IActionResult Edit([FromRoute]int id, OpenCoursesCreateInputModel inputModel)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            
            OpenCourse? openCourse = DbContext
                .OpenCourses
                .Include(oc => oc.Category)
                .SingleOrDefault(oc => oc.Id == id);

            if(openCourse == null)
            {
                return NotFound();
            }
            inputModel.Categories = DbContext
                    .CourseCategories
                    .AsNoTracking()
                    .OrderBy(cc => cc.Name)
                    .ToList();
            if(!ModelState.IsValid)
            {
                return View(inputModel);
            }
            if(CourseCategoryExists(inputModel.CategoryId) == false)
            {
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

                DbContext.OpenCourses.Update(openCourse);
                DbContext.SaveChanges();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                ModelState.AddModelError(string.Empty, "An error occurred while editing the open course. Please try again.");
                return View(inputModel);
            }
        }

        private bool CourseCategoryExists(int id)
        {
            return DbContext.CourseCategories.Any(e => e.Id == id);
        }

        private bool OpenCourseExists(int id)
        {
            return DbContext.OpenCourses.Any(e => e.Id == id);
        }

    }
}
