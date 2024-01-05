using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class BlogViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
        public string? ImagePath { get; set; }
        public string? Category { get; set; }
        public string? Author { get; set; }
        public int CommentCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTime.Now;
        public DateTimeOffset UpdatedAt { get; set; } = DateTime.Now;
        public DateTimeOffset DeletedAt { get; set; } = DateTime.Now;

    }
}
