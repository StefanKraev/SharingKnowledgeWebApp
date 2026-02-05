using Microsoft.AspNetCore.Mvc.Rendering;
using SharingKnowledge.Models;
using System.ComponentModel.DataAnnotations;
using static SharingKnowledge.Common.ValidationConstrains;

namespace SharingKnowledge.ViewModels.Courses
{
    public class OpenCoursesCreateInputModel
    {
        [Required]
        [MinLength(CourseTitleMinLength)]
        [MaxLength(CourseTitleMaxLength)]
        public string Title { get; set; } = null!;
        [Required]
        [MinLength(CourseDescriptionMinLength)]
        [MaxLength(CourseDescriptionMaxLength)]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime StartDate { get; set; }
        [RegularExpression(CourseImageUrlRegularExpression)]
        [MaxLength(CourseImageUrlMaxLength)]
        public string ?ImageUrl { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public IEnumerable<CourseCategory> Categories { get; set; } = new List<CourseCategory>();
    }
}
