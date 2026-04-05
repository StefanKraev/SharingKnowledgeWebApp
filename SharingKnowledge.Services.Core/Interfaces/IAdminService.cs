using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Services.Core.Interfaces
{
    public interface IAdminService
    {
        //Note: This method is made in order to avoid injecting ApplicationDbContext in the admin controller.
        // Ufortunately, the viewmodels used in admin area are unusable here as the web layer depends on services.
        // That is also why this service is so thin.
        Task<bool> DeleteUserWithCleanupAsync(string userId);
    }
}
