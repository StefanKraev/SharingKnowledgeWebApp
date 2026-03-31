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

        [HttpPost]
        public async Task<IActionResult> Enroll(int courseId)
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
                return NotFound();
            }

            OpenCourse course =
                await openCoursesService.GetCourseByIdAsync(courseId);

            if (course == null)
            {
                return NotFound();
            }

            if (!student.EnrolledCourses.Any(c => c.Id == courseId))
            {
                await openCoursesService.AddCourseToStudentAsync(course, student);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Unenroll(int courseId)
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
                return NotFound();
            }

            OpenCourse course =
                await openCoursesService.GetCourseByIdAsync(courseId);

            if (course == null)
            {
                return NotFound();
            }

            await openCoursesService.UnenrollStudentAsync(student, course);

            return RedirectToAction(nameof(Index));
        }
    }
}
