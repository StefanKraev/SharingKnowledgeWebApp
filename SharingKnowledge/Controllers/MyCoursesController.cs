using Microsoft.AspNetCore.Mvc;

namespace SharingKnowledge.Controllers
{
    public class MyCoursesController : ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
