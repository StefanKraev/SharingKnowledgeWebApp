using Microsoft.AspNetCore.Mvc;

namespace SharingKnowledge.Areas.Admin.Controllers
{
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
