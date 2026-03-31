using Microsoft.AspNetCore.Mvc;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.Web.ViewModels.Courses;

namespace SharingKnowledge.Controllers
{
    public class MyCoursesController : ControllerBase
    {
        private readonly IOpenCoursesService openCoursesService;

        public MyCoursesController(IOpenCoursesService openCoursesService)
        {
            this.openCoursesService = openCoursesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? userId = GetUserId();

            if (userId == null)
            {
                return BadRequest();
            }

            Student student =
                await openCoursesService.GetStudentByIdAsync(userId);

            if (student == null)
            {
                return BadRequest();
            }

            ICollection<OpenCoursesMyCoursesViewModel> myCourses =
                await openCoursesService.GetAllStudentCourses(student);

            return View(myCourses);
        }
    }
}
