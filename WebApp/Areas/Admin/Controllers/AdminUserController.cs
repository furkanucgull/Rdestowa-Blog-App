using App.Data;
using App.Data.Entities;
using App.Services.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Claims;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminUserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public AdminUserController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<IActionResult> AddImage([FromRoute] int id, UserViewModel model, [FromForm] IFormFile formFile)
        {
            if (formFile != null)
            {
                if (formFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("File", "The file size cannot be larger than 2 MB.");
                    ViewBag.Error = "The file size cannot be larger than 2 MB.";
                    return RedirectToAction("Edit", "User", new { id = id });
                }

                if (Path.GetExtension(formFile.FileName).ToLower() != ".jpg")
                {
                    ModelState.AddModelError("File", "Only .jpg files");
                    ViewBag.Error = "Only .jpg files";
                    return RedirectToAction("Edit", "User", new { id = id });
                }

                await _fileService.UploadFileAsync(formFile);
                string imageName = formFile.FileName;
                long imageSize = formFile.Length;
                var userId = int.TryParse(User.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;
                var user = _context.Users.FirstOrDefault(x => x.Id.ToString() == userId);
                UserImageEntity userImage = new()
                {
                    CreatedAt = DateTime.Now,
                    Id = id,
                    ImagePath = $"/uploads/{imageName}",
                    UserId = user.Id
                };
                _context.UserImages.Add(userImage);
                await _context.SaveChangesAsync();
                TempData["UploadMessage"] = "Profile Picture Added Successfully";
            }
            return RedirectToAction("AboutUs", "Home", new { area = "" });

        }
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new UserEntity
                {
                    Id = model.Id,
                    Email = model.NewEmail,
                    FacebookAddress = model.FacebookAddress,
                    InstagramAddress = model.InstagramAddress,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Name = model.Name,
                    Phone = model.Phone,
                    UserImagePath = model.UserImagePath,
                    Address = model.Address,
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        //[Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }
            var userImage = _context.UserImages.FirstOrDefault(x => x.UserId == user.Id);
            var imagePath = userImage != null ? userImage.ImagePath : "default_image_path.jpg";
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                NewEmail = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                Address = user.Address,
                FacebookAddress = user.FacebookAddress,
                InstagramAddress = user.InstagramAddress,
                UserImagePath = imagePath
            };

            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, UserViewModel model)
        {


            var user = _context.Users.Find(id);

            if (user != null)
            {

                //user.Email = model.NewEmail;
                user.FacebookAddress = model.FacebookAddress;
                user.InstagramAddress = model.InstagramAddress;
                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;
                user.Name = model.Name;
                //user.Phone = model.Phone;
                user.UserImagePath = model.UserImagePath;
                //user.Address = model.Address;

                TempData["SuccessMessage"] = "Edited Successfully";
                _context.SaveChanges();
                var viewmodel = new UserViewModel
                {
                    Id = id,
                    NewEmail = model.NewEmail,
                    Name = model.Name,
                    Phone = model.Phone,
                    Address = model.Address,
                    FacebookAddress = model.FacebookAddress,
                    InstagramAddress = model.InstagramAddress,
                    UserImagePath = model.UserImagePath,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,

                };
                return View(viewmodel);
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult EditTitle([FromRoute] int id, UserViewModel model)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.Title = model.Title;
                TempData["SuccessMessage"] = "Title Edited Successfully";
                _context.SaveChanges();
            }
            return RedirectToAction("Edit", "AdminUser", new { id = id });
        }
        [HttpPost]
        public IActionResult EditDescription([FromRoute] int id, UserViewModel model)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.Description = model.Description;
                TempData["SuccessMessage"] = "Description Edited Successfully";
                _context.SaveChanges();
            }
            return RedirectToAction("Edit", "AdminUser", new { id = id });
        }
        [HttpPost]
        public IActionResult EditName([FromRoute] int id, UserViewModel model)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.Name = model.Name;
                TempData["SuccessMessage"] = "Name Edited Successfully";
                _context.SaveChanges();
            }
            return RedirectToAction("Edit", "AdminUser", new { id = id });
        }
        [HttpPost]
        public IActionResult EditFacebookAddress([FromRoute] int id, UserViewModel model)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.FacebookAddress = model.FacebookAddress;
                TempData["SuccessMessage"] = "FacebookAddress Edited Successfully";
                _context.SaveChanges();
            }
            return RedirectToAction("Edit", "AdminUser", new { id = id });
        }


        // GET: /Admin/AdminUser/Delete/{id}
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: /Admin/AdminUser/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
