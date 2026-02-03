using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SharingKnowledge.Common.ValidationConstrains;

namespace SharingKnowledge.Models
{
    public class OpenCourse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CourseTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(CourseDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [MaxLength(CourseImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Required]
        [ForeignKey(nameof(CourseCategory))]
        public int CategoryId { get; set; }

        public virtual CourseCategory Category { get; set; } = null!;
    }
}
