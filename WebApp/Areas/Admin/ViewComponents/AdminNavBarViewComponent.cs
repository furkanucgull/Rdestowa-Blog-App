using App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Security.Claims;
using WebApp.Models;

namespace WebApp.Areas.Admin.ViewComponents
{
    public class AdminNavBarViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        public AdminNavBarViewComponent(AppDbContext db)
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

                var navbarList = new NavbarListViewModel
                {
                    Id = user.Id,
                   
                    Name = user.Name,
                    UserImagePath = userImage?.ImagePath,
                };
                return View(navbarList);
            }
            return View();
         
        }
    }
}
