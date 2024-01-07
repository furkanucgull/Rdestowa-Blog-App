using App.Data.Abstract;
using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using WebApp.Areas.Admin.Models;

namespace WebApp.Areas.Admin.ViewComponents

{
    public class AdminLayoutViewComponent : ViewComponent
    {
        private readonly IRepository<UserEntity> _userRepository;

        public AdminLayoutViewComponent(IRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }

        public ViewViewComponentResult Invoke()
        {
            var activeUserCount = GetActiveUserCount();
            var usersCount = GetUserCount();
            var model = new AdminLayoutViewModel
            {
                ActiveUserCount = activeUserCount,
                UserCount = usersCount
            };
            return View(model);
        }

        private int GetActiveUserCount()
        {
            var activeUsers = _userRepository.GetAll().Count(u => u.IsEmailConfirmed);
            return activeUsers;
        }

        private int GetUserCount()
        {
            var uCount = _userRepository.GetAll().Count();
            return uCount;
        }

    }
}
