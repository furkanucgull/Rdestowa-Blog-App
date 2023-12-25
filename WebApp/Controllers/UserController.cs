using Ads.Data.Entities;
using Ads.Web.Mvc.Models;
using App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return View();
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
                UserImagePath = imagePath
            };

            return View(userViewModel);
        }



    }
}
        
    
