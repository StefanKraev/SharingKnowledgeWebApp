using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static SharingKnowledge.Common.ValidationConstrains;

namespace SharingKnowledge.Models
{
    public class Student : IdentityUser
    {
        [Required]
        [MaxLength(StudentFNMaxLen)]
        [RegularExpression(StudentFNRegularExpression)]
        public string FacultyNumber { get; set; } = null!;

        public virtual ICollection<OpenCourse> EnrolledCourses { get; set; } = new List<OpenCourse>();

        public virtual ICollection<OpenCourse> CreatedCourses { get; set; } = new List<OpenCourse>();
    }
}
