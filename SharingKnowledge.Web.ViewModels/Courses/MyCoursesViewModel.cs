using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharingKnowledge.Web.ViewModels.Courses
{
    public class MyCoursesViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        public string? ImageUrl { get; set; }

        [Required]

        public string CategoryName { get; set; } = null!;

        [Required]
        public string CreatorId { get; set; } = null!;

        [Required]

        public bool IsEnrolled { get; set;}
    }
}
