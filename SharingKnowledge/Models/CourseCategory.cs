using System.ComponentModel.DataAnnotations;
using static SharingKnowledge.Common.ValidationConstrains;

namespace SharingKnowledge.Models
{
    public class CourseCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<OpenCourse> Courses { get; set; } = new List<OpenCourse>();
    }
}
