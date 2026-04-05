using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data;
using SharingKnowledge.Data.Models;
using SharingKnowledge.Models;
using SharingKnowledge.Services.Core.Interfaces;

namespace SharingKnowledge.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<bool> DeleteUserWithCleanupAsync(string userId)
        {
            ApplicationUser? user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            } 

            Student? student = await context
                .Students
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student != null)
            {
                int studentId = student.Id;

                List<OpenCourse> userCourses = await context
                    .OpenCourses
                    .Where(c => c.CreatorId == studentId)
                    .ToListAsync();

                if (userCourses.Any())
                {
                    context.OpenCourses.RemoveRange(userCourses);                 
                    await context.SaveChangesAsync();
                }
          
                context.Students.Remove(student);
                await context.SaveChangesAsync();
            }

            var result = await userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}