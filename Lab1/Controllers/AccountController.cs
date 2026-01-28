using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers
{
    public class AccountController : Controller
    { 
        public IActionResult ContentResult()
        {
            return Content("chào thầy nha em là Danh đẹp Trai");
        }

        public IActionResult JsonResult()
        {
            var data = new
            {
                Id = 1,
                Name = "Phan Thành Danh",
                Email = "Danhpttb01287@gmail.com",
                Date = DateTime.Now
            };
            return Json(data);
        }

        public IActionResult Redirect()
        {
            return RedirectToAction("ContentResult");
        }
        public IActionResult File()
        {
            string filePath = "~/images/Danhdeptrai.jpg";
            return File(filePath, "image/jpeg");
        }
    }
}
