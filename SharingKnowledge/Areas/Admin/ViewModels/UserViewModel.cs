namespace SharingKnowledge.Areas.Admin.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; } = null!;

        public List<string> Roles { get; set; } = new List<string>();
    }
}
