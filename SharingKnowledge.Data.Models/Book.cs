using SharingKnowledge.Models;
using static SharingKnowledge.Common.ValidationConstrains;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharingKnowledge.Data.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(BookTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(BookAuthorMaxLength)]
        public string AuthorName { get; set; } = null!; // Could be a separate Author entity, but for simplicity, we store the name directly in the Book entity.

        [Required]
        [MaxLength(BookDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [MaxLength(BookImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryId))]

        public virtual CourseCategory Category { get; set; } = null!;

        public virtual ICollection<StudentBook> UserBooks { get; set; } = new List<StudentBook>();
    }
}
