using App.Data;
using App.Data.Entities;
using App.Services.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public BlogController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            var blogs = _context.BlogPosts.Include(blog => blog.BlogImage).ToList();

            // BlogViewModel listesini oluşturma
            var blogViewModels = blogs.Select(blog => new BlogViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Author = blog.Author,
                CommentCount = blog.CommentCount,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt,
                DeletedAt = blog.DeletedAt,
                ImagePath = blog.BlogImage?.ImagePath, // Eğer BlogImageEntity ile ilişkilendirilmişse resim yolunu al
            }).ToList();
            return View(blogViewModels);
        }
        [Route("postdetails/{id}")]
        public IActionResult Details(int id)
        {
            var blog = _context.BlogPosts.SingleOrDefault(blog => blog.Id == id);
            var blogs = _context.BlogPosts.Include(blog => blog.BlogImage).ToList();
            var blogViewModel = new BlogViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Author = blog.Author,
                ImagePath = blog.BlogImage?.ImagePath,

                DeletedAt = blog.DeletedAt,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt,
                CommentCount = blog.CommentCount
            };
            return View(blogViewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BlogViewModel model, [FromForm] IFormFile formFile)
        {
            if (formFile != null)
            {
                if (formFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("File", "Dosya boyutu 2 MB'dan büyük olamaz.");
                    ViewBag.Error = "Dosya boyutu 2 MB'dan büyük olamaz.";
                    return View("Create", model);
                }

                if (Path.GetExtension(formFile.FileName).ToLower() != ".jpg")
                {
                    ModelState.AddModelError("File", "Sadece .jpg uzantılı dosyaları yükleyebilirsiniz.");
                    ViewBag.Error = "Sadece.jpg uzantılı dosyaları yükleyebilirsiniz.";
                    return View("Create", model);
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            await _fileService.UploadFileAsync(formFile);
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.PrimarySid), out int userId))
            {
                return BadRequest();
            }
            var user = HttpContext.User.Identity.Name;
            BlogEntity newBlog = new()
            {
                Author = user,
                //CommentCount = model.CommentCount,
                Content = model.Content,
                CreatedAt = DateTime.Now,
                DeletedAt = DateTime.Now,
                Title = model.Title,
                UpdatedAt = DateTime.Now,
                UserID = userId,

            };
            _context.BlogPosts.Add(newBlog);
            await _context.SaveChangesAsync();
            string imageName = formFile.FileName;
            string imagePath = $"/uploads/{imageName}";
            long imageSize = formFile.Length;
            BlogImageEntity blgImage = new()
            {
                BlogId = newBlog.Id,
                Blogs = newBlog,
                ImagePath = imagePath,
                ImageSize = imageSize

            };
            _context.BlogImages.Add(blgImage);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Blog post added successfully!";
            return RedirectToAction("Index", "Home");
        }



    }
}


