using App.Data;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;

        public NavbarViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public ViewViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated)
            {
                var claimsPrincipal = User as ClaimsPrincipal;
                var userId = int.TryParse(claimsPrincipal?.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;

                var user = _db.Users.FirstOrDefault(x => x.Id.ToString() == userId);
                var userImage = _db.UserImages.FirstOrDefault(x => x.UserId == user.Id);
                var lastUserImage = _db.UserImages
        .OrderByDescending(u => u.CreatedAt)
        .FirstOrDefault();

                string imagePath;

                if (lastUserImage != null)
                {
                    imagePath = lastUserImage.ImagePath;
                }
                else
                {
                    imagePath = "default_image_path.jpg";
                }

                var navbarList = new NavbarListViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserImagePath = imagePath
                };
                return View(navbarList);
            }
            else { return View(new NavbarListViewModel { Id = 0 }); }
        }
    }
}




