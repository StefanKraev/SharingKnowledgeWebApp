using SharingKnowledge.Models;
using System.ComponentModel.DataAnnotations;
using static SharingKnowledge.Common.ValidationConstrains;

namespace SharingKnowledge.Web.ViewModels.Books
{
    public class BookCreateInputModel
    {
        [Required]
        [MinLength(BookTitleMinLength)]
        [MaxLength(BookTitleMaxLength)]
        [Display(Name = "Book Title")]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(BookAuthorMinLength)]
        [MaxLength(BookAuthorMaxLength)]
        [Display(Name = "Author Name")]
        public string AuthorName { get; set; } = null!;

        [Required]
        [MinLength(BookDescriptionMinLength)]
        [MaxLength(BookDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Url]
        [MaxLength(BookImageUrlMaxLength)]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<CourseCategory> Categories { get; set; } = new List<CourseCategory>();
    }
}