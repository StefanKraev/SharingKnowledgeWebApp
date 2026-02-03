using Microsoft.AspNetCore.Mvc;

namespace SharingKnowledge.Controllers
{
    public class OpenCoursesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
