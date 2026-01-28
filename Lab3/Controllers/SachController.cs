// File: Controllers/SachController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using bai2.Models;
using bai2.ViewModels;
using bai2.Helpers;
using System.Data;

namespace bai2.Controllers
{
    public class SachController : Controller
    {
        private readonly string _connectionString;
        private readonly ILogger<SachController> _logger;

        public SachController(IConfiguration configuration, ILogger<SachController> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        // GET: Sach/Index
        public async Task<IActionResult> Index()
        {
            var sachList = new List<SachViewModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = @"
                    SELECT s.MaSach, s.TieuDe, s.TacGia, s.GiaTien, 
                           s.MoTaChiTiet, s.MaTheLoai, 
                           s.SoLuongTon, s.NgayTao, t.TenTheLoai,
                           CASE WHEN s.AnhBia IS NOT NULL THEN 1 ELSE 0 END AS CoAnhBia
                    FROM Sach s 
                    INNER JOIN TheLoai t ON s.MaTheLoai = t.MaTheLoai 
                    ORDER BY s.NgayTao DESC";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var sach = new SachViewModel
                            {
                                MaSach = reader.GetInt32(0),
                                TieuDe = reader.GetString(1),
                                TacGia = reader.GetString(2),
                                GiaTien = reader.GetDecimal(3),
                                MoTaChiTiet = reader.IsDBNull(4) ? null : reader.GetString(4),
                                MaTheLoai = reader.GetInt32(5),
                                SoLuongTon = reader.GetInt32(6),
                                NgayTao = reader.GetDateTime(7),
                                TenTheLoai = reader.GetString(8),
                                CoAnhBia = reader.GetInt32(9) == 1
                            };
                            sachList.Add(sach);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading books index");
                TempData["Error"] = "Có lỗi xảy ra khi tải danh sách sách";
            }

            return View(sachList);
        }

        // GET: Sach/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SachViewModel sach = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = @"
                    SELECT s.MaSach, s.TieuDe, s.TacGia, s.GiaTien, 
                           s.MoTaChiTiet, s.MaTheLoai, 
                           s.SoLuongTon, s.NgayTao, t.TenTheLoai,
                           CASE WHEN s.AnhBia IS NOT NULL THEN 1 ELSE 0 END AS CoAnhBia
                    FROM Sach s 
                    INNER JOIN TheLoai t ON s.MaTheLoai = t.MaTheLoai 
                    WHERE s.MaSach = @MaSach";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaSach", id);

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            sach = new SachViewModel
                            {
                                MaSach = reader.GetInt32(0),
                                TieuDe = reader.GetString(1),
                                TacGia = reader.GetString(2),
                                GiaTien = reader.GetDecimal(3),
                                MoTaChiTiet = reader.IsDBNull(4) ? null : reader.GetString(4),
                                MaTheLoai = reader.GetInt32(5),
                                SoLuongTon = reader.GetInt32(6),
                                NgayTao = reader.GetDateTime(7),
                                TenTheLoai = reader.GetString(8),
                                CoAnhBia = reader.GetInt32(9) == 1
                            };
                        }
                    }
                }

                if (sach == null)
                {
                    return NotFound();
                }

                return View(sach);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading book details for ID {MaSach}", id);
                TempData["Error"] = "Có lỗi xảy ra khi tải chi tiết sách";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Sach/Create
        public IActionResult Create()
        {
            var viewModel = new CreateSachViewModel
            {
                TheLoaiList = GetTheLoaiList()
            };

            return View(viewModel);
        }

        // POST: Sach/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSachViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.TheLoaiList = GetTheLoaiList();
                return View(viewModel);
            }

            try
            {
                byte[] anhBiaBytes = null;

                // Xử lý upload ảnh
                if (viewModel.AnhBiaFile != null && viewModel.AnhBiaFile.Length > 0)
                {
                    anhBiaBytes = await ProcessImageUploadAsync(viewModel.AnhBiaFile);
                }

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = @"
                    INSERT INTO Sach (TieuDe, TacGia, GiaTien, MoTaChiTiet, AnhBia, MaTheLoai, SoLuongTon, NgayTao)
                    VALUES (@TieuDe, @TacGia, @GiaTien, @MoTaChiTiet, @AnhBia, @MaTheLoai, @SoLuongTon, GETDATE())";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@TieuDe", viewModel.TieuDe);
                    cmd.Parameters.AddWithValue("@TacGia", viewModel.TacGia);
                    cmd.Parameters.AddWithValue("@GiaTien", viewModel.GiaTien);
                    cmd.Parameters.AddWithValue("@MoTaChiTiet", (object)viewModel.MoTaChiTiet ?? DBNull.Value);
                    cmd.Parameters.Add("@AnhBia", SqlDbType.VarBinary, -1).Value = (object)anhBiaBytes ?? DBNull.Value;
                    cmd.Parameters.AddWithValue("@MaTheLoai", viewModel.MaTheLoai);
                    cmd.Parameters.AddWithValue("@SoLuongTon", viewModel.SoLuongTon);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }

                TempData["Success"] = "Thêm sách thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book");
                TempData["Error"] = "Có lỗi xảy ra khi thêm sách: " + ex.Message;
                viewModel.TheLoaiList = GetTheLoaiList();
                return View(viewModel);
            }
        }

        // GET: Sach/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EditSachViewModel viewModel = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = @"
                    SELECT s.MaSach, s.TieuDe, s.TacGia, s.GiaTien, 
                           s.MoTaChiTiet, s.MaTheLoai, s.SoLuongTon,
                           CASE WHEN s.AnhBia IS NOT NULL THEN 1 ELSE 0 END AS CoAnhBia
                    FROM Sach s 
                    WHERE s.MaSach = @MaSach";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaSach", id);

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            viewModel = new EditSachViewModel
                            {
                                MaSach = reader.GetInt32(0),
                                TieuDe = reader.GetString(1),
                                TacGia = reader.GetString(2),
                                GiaTien = reader.GetDecimal(3),
                                MoTaChiTiet = reader.IsDBNull(4) ? null : reader.GetString(4),
                                MaTheLoai = reader.GetInt32(5),
                                SoLuongTon = reader.GetInt32(6),
                                CoAnhBia = reader.GetInt32(7) == 1,
                                TheLoaiList = GetTheLoaiList()
                            };
                        }
                    }
                }

                if (viewModel == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading book edit for ID {MaSach}", id);
                TempData["Error"] = "Có lỗi xảy ra khi tải trang sửa sách";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Sach/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditSachViewModel viewModel)
        {
            if (id != viewModel.MaSach)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                viewModel.TheLoaiList = GetTheLoaiList();
                return View(viewModel);
            }

            try
            {
                byte[] anhBiaBytes = null;
                bool updateImage = false;

                // Nếu có upload ảnh mới
                if (viewModel.AnhBiaFile != null && viewModel.AnhBiaFile.Length > 0)
                {
                    anhBiaBytes = await ProcessImageUploadAsync(viewModel.AnhBiaFile);
                    updateImage = true;
                }

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string sql;

                    if (updateImage)
                    {
                        // Cập nhật cả ảnh
                        sql = @"
                        UPDATE Sach 
                        SET TieuDe = @TieuDe, TacGia = @TacGia, GiaTien = @GiaTien, 
                            MoTaChiTiet = @MoTaChiTiet, AnhBia = @AnhBia,
                            MaTheLoai = @MaTheLoai, SoLuongTon = @SoLuongTon
                        WHERE MaSach = @MaSach";
                    }
                    else
                    {
                        // Không cập nhật ảnh
                        sql = @"
                        UPDATE Sach 
                        SET TieuDe = @TieuDe, TacGia = @TacGia, GiaTien = @GiaTien, 
                            MoTaChiTiet = @MoTaChiTiet, MaTheLoai = @MaTheLoai, 
                            SoLuongTon = @SoLuongTon
                        WHERE MaSach = @MaSach";
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaSach", viewModel.MaSach);
                    cmd.Parameters.AddWithValue("@TieuDe", viewModel.TieuDe);
                    cmd.Parameters.AddWithValue("@TacGia", viewModel.TacGia);
                    cmd.Parameters.AddWithValue("@GiaTien", viewModel.GiaTien);
                    cmd.Parameters.AddWithValue("@MoTaChiTiet", (object)viewModel.MoTaChiTiet ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaTheLoai", viewModel.MaTheLoai);
                    cmd.Parameters.AddWithValue("@SoLuongTon", viewModel.SoLuongTon);

                    if (updateImage)
                    {
                        cmd.Parameters.Add("@AnhBia", SqlDbType.VarBinary, -1).Value = (object)anhBiaBytes ?? DBNull.Value;
                    }

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }

                TempData["Success"] = "Cập nhật sách thành công";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book {MaSach}", id);
                TempData["Error"] = "Có lỗi xảy ra khi cập nhật sách: " + ex.Message;
                viewModel.TheLoaiList = GetTheLoaiList();
                return View(viewModel);
            }
        }

        // GET: Sach/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SachViewModel sach = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = @"
                    SELECT s.MaSach, s.TieuDe, s.TacGia, s.GiaTien, 
                           s.MoTaChiTiet, s.MaTheLoai, 
                           s.SoLuongTon, s.NgayTao, t.TenTheLoai,
                           CASE WHEN s.AnhBia IS NOT NULL THEN 1 ELSE 0 END AS CoAnhBia
                    FROM Sach s 
                    INNER JOIN TheLoai t ON s.MaTheLoai = t.MaTheLoai 
                    WHERE s.MaSach = @MaSach";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaSach", id);

                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            sach = new SachViewModel
                            {
                                MaSach = reader.GetInt32(0),
                                TieuDe = reader.GetString(1),
                                TacGia = reader.GetString(2),
                                GiaTien = reader.GetDecimal(3),
                                MoTaChiTiet = reader.IsDBNull(4) ? null : reader.GetString(4),
                                MaTheLoai = reader.GetInt32(5),
                                SoLuongTon = reader.GetInt32(6),
                                NgayTao = reader.GetDateTime(7),
                                TenTheLoai = reader.GetString(8),
                                CoAnhBia = reader.GetInt32(9) == 1
                            };
                        }
                    }
                }

                if (sach == null)
                {
                    return NotFound();
                }

                return View(sach);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading book delete for ID {MaSach}", id);
                TempData["Error"] = "Có lỗi xảy ra khi tải trang xóa sách";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Sach/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = "DELETE FROM Sach WHERE MaSach = @MaSach";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaSach", id);

                    await conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }

                TempData["Success"] = "Xóa sách thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book {MaSach}", id);
                TempData["Error"] = "Có lỗi xảy ra khi xóa sách: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Sach/GetImage/5 - Lấy ảnh từ database
        public async Task<IActionResult> GetImage(int id)
        {
            try
            {
                byte[] anhBia = null;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = "SELECT AnhBia FROM Sach WHERE MaSach = @MaSach";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaSach", id);

                    await conn.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        anhBia = (byte[])result;
                    }
                }

                if (anhBia == null || anhBia.Length == 0)
                {
                    // Trả về ảnh mặc định
                    return NotFound();
                }

                // Sử dụng ImageHelper để detect content type
                string contentType = ImageHelper.GetContentType(anhBia);
                return File(anhBia, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading image {MaSach}", id);
                return NotFound();
            }
        }

        // ==================== HELPER METHODS ====================

        /// <summary>
        /// Xử lý upload và validate ảnh
        /// </summary>
        private async Task<byte[]> ProcessImageUploadAsync(IFormFile imageFile)
        {
            // Kiểm tra kích thước file
            if (imageFile.Length > 5 * 1024 * 1024) // 5MB
            {
                throw new InvalidOperationException("Kích thước ảnh không được vượt quá 5MB");
            }

            // Kiểm tra định dạng file
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("Chỉ chấp nhận file ảnh (JPG, JPEG, PNG, GIF, WEBP, BMP)");
            }

            // Chuyển đổi file sang byte array
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Lấy danh sách thể loại cho dropdown
        /// </summary>
        private List<SelectListItem> GetTheLoaiList()
        {
            var theLoaiList = new List<SelectListItem>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = "SELECT MaTheLoai, TenTheLoai FROM TheLoai ORDER BY TenTheLoai";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            theLoaiList.Add(new SelectListItem
                            {
                                Value = reader["MaTheLoai"].ToString(),
                                Text = reader["TenTheLoai"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading categories");
            }

            return theLoaiList;
        }
    }
}