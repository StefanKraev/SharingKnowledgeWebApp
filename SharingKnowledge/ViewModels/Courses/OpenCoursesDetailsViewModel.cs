using System.ComponentModel.DataAnnotations;

namespace SharingKnowledge.ViewModels.Courses
{
    public class OpenCoursesDetailsViewModel
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        public string? ImageUrl { get; set; }

        [Required]

        public string CategoryName { get; set; } = null!;
    }
}
