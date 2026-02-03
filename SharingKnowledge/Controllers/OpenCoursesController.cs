using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SharingKnowledge.Data;
using SharingKnowledge.Models;
using SharingKnowledge.ViewModels.Courses;
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
            OpenCourse? openCourse = DbContext
                .OpenCourses
                .AsNoTracking()
                .FirstOrDefault(oc => oc.Id == id);
            if (openCourse == null)
            {
                return NotFound();
            }
            return View(openCourse);
        }
    }
}
