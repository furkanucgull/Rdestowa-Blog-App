using App.Data;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var blogs = _context.BlogPosts.ToList();

            var blogViewModels = blogs.Select(blog => new BlogViewModel
            {
                Id = blog.PostID,
                Title = blog.Title,
                Content = blog.Content,
                Author = blog.Author,
                DeletedAt = blog.DeletedAt,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt,
                Category = blog.Category,
                CommentCount = blog.CommentCount,
            }).ToList();

            return View(blogViewModels);
        }
        [Route("postdetails/{id}")]
        public IActionResult Details(int id)
        {
            var blog = _context.BlogPosts.SingleOrDefault(blog => blog.PostID == id);
            var blogViewModel = new BlogViewModel
            {
                Id = blog.PostID,
                Title = blog.Title,
                Content = blog.Content,
                Author = blog.Author,
                DeletedAt = blog.DeletedAt,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt,
                Category = blog.Category,
                CommentCount = blog.CommentCount
            };
            return View(blogViewModel);
        }
    }
}
