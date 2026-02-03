using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SharingKnowledge.Data;
using SharingKnowledge.Models;

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

            return View();
        }
    }
}
