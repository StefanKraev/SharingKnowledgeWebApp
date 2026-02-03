using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharingKnowledge.Models
{
    public class OpenCourse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        [ForeignKey(nameof(CourseCategory))]
        public int CategoryId { get; set; }

        public virtual CourseCategory Category { get; set; } = null!;
    }
}
