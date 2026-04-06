using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Web.ViewModels.Books
{
    public class DetailsBookViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string AuthorName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string CategoryName { get; set; } = null!;

        public string PublisherId { get; set; } = null!;
    }
}
