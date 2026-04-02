using SharingKnowledge.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharingKnowledge.Data.Models
{
    public class StudentBook
    {
        public int StudentId { get; set; }

        [Required]
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; } = null!;

        public int BookId { get; set; }
        [Required]
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; } = null!;

        public DateTime AddedToLibraryDate { get; set; }

        [Required]
        public bool IsRead { get; set; } = false;
    }
}
