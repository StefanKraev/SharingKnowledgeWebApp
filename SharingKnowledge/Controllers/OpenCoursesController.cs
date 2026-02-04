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
                    Description = oc.Description,
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
            OpenCoursesCreateInputModel openCoursesCreateInputModel = new OpenCoursesCreateInputModel();

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
            if(id <= 0)
            {
                return BadRequest();
            }
        }

        private bool CourseCategoryExists(int id)
        {
            return DbContext.CourseCategories.Any(e => e.Id == id);
        }

    }
}
