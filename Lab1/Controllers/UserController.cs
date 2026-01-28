using Microsoft.AspNetCore.Mvc;
using Lab1.Models;

namespace Lab1.Controllers;

public class UserController : Controller
{
    // Static list to simulate database
    private static List<User> _users = new List<User>
    {
        new User { UserId = 1, Username = "admin", Email = "admin@example.com", Password = "Password123" },
        new User { UserId = 2, Username = "user1", Email = "user1@example.com", Password = "Password123" }
    };

    public IActionResult Index()    
    {
        return View(_users);
    }

    [HttpGet]
    public IActionResult GetUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            TempData["Error"] = "Không tìm thấy người dùng!";
            return RedirectToAction("Index");
        }
        return View(user); // Reusing Details view or similar
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateUser(User user)
    {
        if (ModelState.IsValid)
        {
            user.UserId = _users.Count > 0 ? _users.Max(u => u.UserId) + 1 : 1;
            // In a real app, hash the password here
            _users.Add(user);
            TempData["Success"] = "Tạo người dùng thành công!";
            return RedirectToAction("Index");
        }
        TempData["Error"] = "Tạo người dùng thất bại. Vui lòng kiểm tra lại.";
        return View("Create", user);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var user = _users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            TempData["Error"] = "Không tìm thấy người dùng!";
            return RedirectToAction("Index");
        }
        return View(user);
    }

    [HttpPut]
    [ActionName("ModifyUser")]
    public IActionResult UpdateUser(int id, User updatedUser)
    {
        // Note: For HTML forms, PUT is not supported natively without JS or method override. 
        // MVC often uses Post for updates. But requirement says HttpPut.
        // We can simulate this call or use fetch, but standard form will be POST.
        // For this lab, if called via API/Postman it works. 
        // For UI, we might need to stick to POST or use a hidden field if supported, 
        // but let's implement as requested and also allow POST for form compatibility if needed 
        // or just strict PUT as requested. 
        // Requirement: "ModifyUser (HttpPut): Use [ActionName("ModifyUser")] to rename UpdateUser."
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = _users.FirstOrDefault(u => u.UserId == id);
        if (user != null)
        {
            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;
            TempData["Success"] = "Cập nhật người dùng thành công!";
            return RedirectToAction("Index");
        }
        
        TempData["Error"] = "Không tìm thấy người dùng!";
        return NotFound();
    }

    // Creating a POST wrapper for the UI form to call Update
    [HttpPost]
    public IActionResult Edit(int id, User updatedUser)
    {
        if (!ModelState.IsValid)
        {
            return View(updatedUser);
        }
        // Calling the business logic of UpdateUser
        // In a real API scenario, we'd separate logic from controller actions.
        var user = _users.FirstOrDefault(u => u.UserId == id);
        if (user != null)
        {
            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;
            TempData["Success"] = "Cập nhật người dùng thành công!";
            return RedirectToAction("Index");
        }
        TempData["Error"] = "Cập nhật thất bại!";
        return View(updatedUser);
    }

    [HttpPost] // Using POST for delete from form, or HttpDelete for API
    public IActionResult DeleteUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.UserId == id);
        if (user != null)
        {
            _users.Remove(user);
            TempData["Success"] = "Đã xóa người dùng!";
        }
        else
        {
            TempData["Error"] = "Không tìm thấy người dùng!";
        }
        return RedirectToAction("Index");
    }

    [NonAction]
    private string GenerateSecurePassword()
    {
        return "SecurePa$$word_" + Guid.NewGuid().ToString().Substring(0, 8);
    }
}