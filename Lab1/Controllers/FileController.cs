using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers;

public class FileController : Controller
{
    private readonly IWebHostEnvironment _env;

    public FileController(IWebHostEnvironment env)
    {
        _env = env;
    }

    public IActionResult Index()
    {
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var files = Directory.GetFiles(uploadsFolder)
                             .Select(f => Path.GetFileName(f))
                             .ToList();

        return View(files);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            TempData["Success"] = "Tải tệp lên thành công!";
        }
        else
        {
            TempData["Error"] = "Vui lòng chọn một tệp hợp lệ.";
        }

        return RedirectToAction("Index");
    }

    public IActionResult Download(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return RedirectToAction("Index");
        }

        var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
        if (System.IO.File.Exists(filePath))
        {
            // Lab Requirement: Use PhysicalFileResult
            return PhysicalFile(filePath, "application/octet-stream", fileName);
        }

        TempData["Error"] = "Không tìm thấy tệp!";
        return RedirectToAction("Index");
    }

    // --- DEMO ACTIONS FOR LAB 3 ---

    // 1. ContentResult Demo
    public IActionResult DemoContent()
    {
        return Content("Thầy Khải đẹp trai cho em 10 điểm nhé .", "text/plain; charset=utf-8");
    }

    // 2. JsonResult Demo
    public IActionResult DemoJson()
    {
         var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
         var files = Directory.Exists(uploadsFolder) 
                    ? Directory.GetFiles(uploadsFolder).Select(Path.GetFileName).ToList() 
                    : new List<string>();

        var data = new
        {
            Message = "ví dụ về JsonResult",
            TotalFiles = files.Count,
            FileList = files,
            Time = DateTime.Now
        };
        return Json(data);
    }

    // 3. FileResult Demo (Existing Download but explicitly named for demo purpose if needed, otherwise Download covers it)
    // We already have Download, so maybe a specific one for images or texts if desired, but Download is the main one.
    // Let's create a stream download demo.
    public IActionResult DemoFileStream()
    {
        // Creates a simple text file in memory and returns it
        var content = "Phan Thành Danh .";
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        return File(stream, "text/plain; charset=utf-8", "demo_stream.txt");
    }

    // 4. RedirectResult Demo
    public IActionResult DemoRedirect()
    {
        // Redirects to Google
        return Redirect("https://www.facebook.com/thanh.danh.phan.941321?locale=vi_VN");
    }
    
    // 5. Build-in RedirectToActionResult Demo (Local)
    public IActionResult DemoRedirectLocal()
    {
        TempData["Success"] = "Đã chuyển hướng từ DemoRedirectLocal về Index!";
        return RedirectToAction("Index");
    }
}