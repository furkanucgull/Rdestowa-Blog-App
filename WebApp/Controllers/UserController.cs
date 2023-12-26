using App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
        [Route("details/{id}")]
        public IActionResult Detail(int id)
        {
            var user = _context.Users.Include(u => u.UserImages).FirstOrDefault(u => u.Id == id);
            var userImage = _context.UserImages.FirstOrDefault(x => x.UserId == user.Id);
            var imagePath = userImage != null ? userImage.ImagePath : "default_image_path.jpg";

            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                NewEmail = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                InstagramAddress = user.InstagramAddress,
                FacebookAddress = user.FacebookAddress,
                UserImagePath = imagePath
            };

            return View(userViewModel);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                NewEmail = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                InstagramAddress = user.InstagramAddress,
                FacebookAddress = user.FacebookAddress
            };

            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Find(id);

                if (user == null)
                {
                    return NotFound();
                }

                user.Name = model.Name;
                user.Email = model.NewEmail;
                user.Address = model.Address;
                user.Phone = model.Phone;
                user.InstagramAddress = model.InstagramAddress;
                user.FacebookAddress = model.FacebookAddress;

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }



    }
}


