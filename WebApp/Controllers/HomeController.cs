﻿using App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            var users = _context.Users.Include(u => u.UserImages).ToList();

            if (users.Count == 0)
            {
                return View("NotFound");
            }

            var userViewModels = users.Select(user =>
            {
                var userImage = _context.UserImages.FirstOrDefault(x => x.UserId == user.Id);
                var imagePath = userImage != null ? userImage.ImagePath : "~/images/defaultuser.png";

                return new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    NewEmail = user.Email,
                    Address = user.Address,
                    Phone = user.Phone,
                    UserImagePath = imagePath
                };
            }).ToList();

            return View(userViewModels);
        }
        public IActionResult ContactUs(int id)
        {
            var users = _context.Users.Include(u => u.UserImages).ToList();

            if (users.Count == 0)
            {
                return View("NotFound");
            }

            var userViewModels = users.Select(user =>
            {
                var userImage = _context.UserImages.FirstOrDefault(x => x.UserId == user.Id);
                var imagePath = userImage != null ? userImage.ImagePath : "~/images/defaultuser.png";

                return new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Title = user.Title,
                    Description = user.Description,
                    NewEmail = user.Email,
                    Address = user.Address,
                    Phone = user.Phone,
                    UserImagePath = imagePath
                };
            }).ToList();

            return View(userViewModels);
        }


    }
}
