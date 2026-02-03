using System.ComponentModel.DataAnnotations;

namespace SharingKnowledge.Models
{
    public class CourseCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public virtual ICollection<OpenCourse> Courses { get; set; } = new List<OpenCourse>();
    }
}
