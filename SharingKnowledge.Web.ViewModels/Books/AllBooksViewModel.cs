using System.ComponentModel.DataAnnotations;

namespace SharingKnowledge.Web.ViewModels.Books
{
    public class AllBooksViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string AuthorName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }

        [Required]

        public string CategoryName { get; set; } = null!;

        public bool IsInUserLibrary { get; set; }
    }
}
