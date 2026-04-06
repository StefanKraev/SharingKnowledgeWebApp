using Microsoft.AspNetCore.Mvc;

namespace SharingKnowledge.Controllers
{
    public class LibraryController : ControllerBase
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
