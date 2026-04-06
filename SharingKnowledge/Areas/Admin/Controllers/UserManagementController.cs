using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.SolutionPersistence.Model;
using SharingKnowledge.Areas.Admin.ViewModels;
using SharingKnowledge.Data.Models;
using SharingKnowledge.Services.Core.Interfaces;

namespace SharingKnowledge.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly IAdminService adminService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserManagementController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IAdminService adminService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = await userManager.Users.ToListAsync();

            List<UserViewModel> userViewModels = new List<UserViewModel>();

            foreach (ApplicationUser user in users)
            {
                IList<string> roles = await userManager.GetRolesAsync(user);

                UserViewModel viewModel = new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? "No Email",
                    Roles = roles.ToList()
                };

                userViewModels.Add(viewModel);
            }

            return View(userViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser? user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            IList<string> userRoles = await userManager.GetRolesAsync(user);

            var allRoles = await roleManager
                .Roles
                .Select(r => r.Name)
                .ToListAsync();

            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email!,
                UserName = user.UserName,
                IsEmailConfirmed = user.EmailConfirmed,
                AvailableRoles = allRoles,
                SelectedRole = userRoles.FirstOrDefault() ?? ""
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            } 

            ApplicationUser? user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;

            IdentityResult updateResult = await userManager.UpdateAsync(user);

            IList<string> currentRoles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!string.IsNullOrEmpty(model.SelectedRole))
            {
                IdentityResult result = await userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            } 

            var currentUserId = userManager.GetUserId(User);

            if (id == currentUserId)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            if (id == userManager.GetUserId(User))
            {
                TempData["ErrorMessage"] = "Self-deletion is not allowed.";
                return RedirectToAction(nameof(Index));
            }

            bool success = await adminService.DeleteUserWithCleanupAsync(id);

            if (success)
            {
                TempData["SuccessMessage"] = "User and all associated data deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
   
            ApplicationUser? user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ModelState.AddModelError("", "An error occurred while deleting the user. Ensure all dependencies are cleared.");
            return View(user);
        }
    }
}
