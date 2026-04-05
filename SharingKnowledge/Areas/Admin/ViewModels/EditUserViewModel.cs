using System.ComponentModel.DataAnnotations;

namespace SharingKnowledge.Areas.Admin.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public string SelectedRole { get; set; } = null!;
        public List<string> AvailableRoles { get; set; } = new List<string>();
    }
}
