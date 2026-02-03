using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SharingKnowledge.Data;
using SharingKnowledge.Models;
using SharingKnowledge.ViewModels;

namespace SharingKnowledge.Controllers
{
    public class OpenCoursesController : Controller
    {
        private readonly ApplicationDbContext DbContext;
        public OpenCoursesController(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<OpenCourse> openCourses = DbContext
                .OpenCourses
                .AsNoTracking()
                .Include(oc => oc.Category)
                .ToList();

            IEnumerable<OpenCoursesAllViewModel> coursesAllViewModels = openCourses
                .Select(oc => new OpenCoursesAllViewModel
                {
                    Title = oc.Title,
                    Description = oc.Description,
                    StartDate = oc.StartDate,
                    ImageUrl = oc.ImageUrl,
                    CategoryName = oc.Category.Name
                })
                .ToList();

            return View(coursesAllViewModels);
        }
    }
}
