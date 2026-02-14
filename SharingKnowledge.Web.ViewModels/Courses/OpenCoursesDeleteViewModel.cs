using System.ComponentModel.DataAnnotations;

namespace SharingKnowledge.ViewModels.Courses
{
    public class OpenCoursesDeleteViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;
    }
}
