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

        public async Task<IActionResult> Edit(int id)
        {
            // Veritabanından düzenlenecek blogu al
            var blog = await _context.BlogPosts.FindAsync(id);
            var currentBlogImage = await _context.BlogImages.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }


            var model = new BlogViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Author = blog.Author,
                CommentCount = blog.CommentCount,
                ImagePath = blog.BlogImage?.ImagePath,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt,
                DeletedAt = blog.DeletedAt
            };
          

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BlogViewModel model, IFormFile formFile)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var blog = await _context.BlogPosts
                    .Include(b => b.BlogImage)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (blog == null)
                {
                    return NotFound();
                }

                blog.Title = model.Title;
                blog.Content = model.Content;
                blog.UpdatedAt = DateTime.Now;

                if (formFile != null)
                {
                    if (blog.BlogImage != null)
                    {
                       
                        _context.BlogImages.Remove(blog.BlogImage);
                    }

                    string imageName = formFile.FileName;
                    var imagePath = $"/uploads/{imageName}";
                    var imageSize = formFile.Length;

                    blog.BlogImage = new BlogImageEntity
                    {
                        ImagePath = imagePath,
                        ImageSize = imageSize
                    };
                }

                _context.Update(blog);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["SuccessMessage"] = "Blog post updated successfully!";
            return RedirectToAction("Edit", "Blog", new { id });
        }

        private bool BlogExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _context.BlogPosts.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }

            if (blog.BlogImage != null)
            {
                _context.BlogImages.Remove(blog.BlogImage);
            }

            _context.BlogPosts.Remove(blog);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Blog post deleted successfully!";
            return RedirectToAction("Index", "Blog");
        }

    }
}


