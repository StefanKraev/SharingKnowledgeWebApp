using Microsoft.AspNetCore.Mvc;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.Web.ViewModels.Courses;

namespace SharingKnowledge.Controllers
{
    public class MyCoursesController : ControllerBase
    {
        private readonly IMyCoursesService myCoursesService;

        public MyCoursesController(IMyCoursesService myCoursesService)
        {
            this.myCoursesService = myCoursesService;
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
                await myCoursesService.GetStudentByIdAsync(userId);

            if (student == null)
            {
                return BadRequest();
            }

            ICollection<MyCoursesViewModel> myCourses =
                await myCoursesService.GetAllStudentCourses(student);

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
                await myCoursesService.GetStudentByIdAsync(userId);

            if (student == null)
            {
                return NotFound();
            }

            OpenCourse course =
                await myCoursesService.GetCourseByIdAsync(courseId);

            if (course == null)
            {
                return NotFound();
            }

            if (!student.EnrolledCourses.Any(c => c.Id == courseId))
            {
                await myCoursesService.AddCourseToStudentAsync(course, student);
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
                await myCoursesService.GetStudentByIdAsync(userId);

            if (student == null)
            {
                return NotFound();
            }

            OpenCourse course =
                await myCoursesService.GetCourseByIdAsync(courseId);

            if (course == null)
            {
                return NotFound();
            }

            await myCoursesService.UnenrollStudentAsync(student, course);

            return RedirectToAction(nameof(Index));
        }
    }
}
