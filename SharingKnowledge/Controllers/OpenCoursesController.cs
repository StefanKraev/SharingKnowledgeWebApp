using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SharingKnowledge.Data;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.ViewModels.Courses;
using SharingKnowledge.Web.ViewModels.Courses;
using System.Diagnostics.Contracts;
using System.Security.Claims;

namespace SharingKnowledge.Controllers
{
    public class OpenCoursesController : ControllerBase
    {
        private readonly IOpenCoursesService openCoursesService;

        public OpenCoursesController(IOpenCoursesService openCoursesService)
        {
            this.openCoursesService = openCoursesService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            string? userId = GetUserId();

            if(userId == null)
            {
                return BadRequest();
            }

            IEnumerable<OpenCoursesMyCoursesViewModel> coursesAllViewModels =
                await openCoursesService.GetAllCoursesAsync(userId);

            return View(coursesAllViewModels);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if(id <= 0)
            {
                return BadRequest();
            }

            OpenCoursesDetailsViewModel? openCourse = 
                await openCoursesService.GetCourseDetailsAsync(id);

            if (openCourse == null)
            {
                return NotFound();
            }

            return View(openCourse);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            OpenCoursesCreateInputModel openCoursesCreateInputModel = 
                await openCoursesService.CreateCourseInput();

            return View(openCoursesCreateInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OpenCoursesCreateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                inputModel.Categories = (await openCoursesService.GetCategoriesAsync()).ToList();
                return View(inputModel);
            }

            if (await openCoursesService.CategoryExistsAsync(inputModel.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(inputModel.CategoryId), "Selected category does not exist.");
                inputModel.Categories = (await openCoursesService.GetCategoriesAsync()).ToList();
                return View(inputModel);
            }

            string? userId = GetUserId();

            if(string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                await openCoursesService.CreateCourseAsync(inputModel, userId);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                ModelState.AddModelError(string.Empty, "An error occurred while creating the open course. Please try again.");
                inputModel.Categories = (await openCoursesService.GetCategoriesAsync()).ToList();
                return View(inputModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            string? userId = GetUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            OpenCoursesCreateInputModel ?inputModel = 
                await openCoursesService.GetCourseForEditAsync(id, userId);

            if (inputModel == null)
            {
                return NotFound();
            }

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int id, OpenCoursesCreateInputModel inputModel)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            string? userId = GetUserId();

            if (userId == null) 
            {
                return RedirectToAction("Login", "Account"); ;
            }

            bool categoryExists = 
                await openCoursesService.CategoryExistsAsync(inputModel.CategoryId);

            if (!ModelState.IsValid || !categoryExists)
            {
                if (!categoryExists)
                {
                    ModelState.AddModelError(nameof(inputModel.CategoryId), "Selected category does not exist.");
                }

                inputModel.Categories = 
                    (await openCoursesService.GetCategoriesAsync()).ToList();

                return View(inputModel);
            }

            bool isSuccess = 
                await openCoursesService.EditCourseAsync(id, inputModel, userId);

            if (!isSuccess)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            string? userId = GetUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            OpenCoursesDeleteViewModel? viewModel =
                await openCoursesService.GetCourseForDeleteAsync(id, userId);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, OpenCoursesDeleteViewModel viewModel)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            string? userId = GetUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            bool isDeleted = 
                await openCoursesService.DeleteCourseAsync(id, userId);

            if (!isDeleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            string? userId = GetUserId();

            if (userId == null)
            {
                return BadRequest();
            }

            Student student = 
                await openCoursesService.GetStudentByIdAsync(userId);

            if(student == null)
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

            if(userId == null)
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

            return RedirectToAction("MyCourses");
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

            return RedirectToAction(nameof(MyCourses));
        }
    }
}
