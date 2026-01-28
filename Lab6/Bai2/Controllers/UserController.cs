using Bai2.Models;
using Bai2.Services;
using Bai2.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bai2.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILoggingService _loggingService;
        private readonly IEmailService _emailService;

        public UserController(
            IUserService userService,
            ILoggingService loggingService,
            IEmailService emailService
        )
        {
            _userService = userService;
            _loggingService = loggingService;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var users = _userService.GetUsers();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            _userService.AddUser(user);
            _loggingService.Log($"Thêm người dùng mới: {user.Email}");
            _emailService.SendEmail(user.Email, "Welcome", "Chào mừng bạn đến với hệ thống!");

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            _userService.UpdateUser(user);
            _loggingService.Log($"Cập nhật người dùng: {user.Email}");
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _userService.DeleteUser(id);
            _loggingService.Log($"Xóa người dùng ID: {id}");
            return RedirectToAction("Index");
        }
    }
}
