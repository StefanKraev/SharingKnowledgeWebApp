using Microsoft.AspNetCore.Identity;
using SharingKnowledge.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SharingKnowledge.Common.ValidationConstrains;

namespace SharingKnowledge.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        [MaxLength(StudentFNMaxLen)]
        [RegularExpression(StudentFNRegularExpression)]
        public string FacultyNumber { get; set; } = null!;

        public virtual ICollection<OpenCourse> EnrolledCourses { get; set; } = new List<OpenCourse>();

        public virtual ICollection<OpenCourse> CreatedCourses { get; set; } = new List<OpenCourse>();

        public virtual ICollection<StudentBook> StudentBooks { get; set; } = new List<StudentBook>();
    }
}
