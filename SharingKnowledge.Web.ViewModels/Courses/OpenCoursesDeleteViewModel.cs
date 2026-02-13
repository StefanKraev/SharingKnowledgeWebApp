using System.ComponentModel.DataAnnotations;

namespace SharingKnowledge.ViewModels.Courses
{
    public class OpenCoursesDeleteViewModel
    {
        [Required]
        public string Title { get; set; } = null!;
    }
}
