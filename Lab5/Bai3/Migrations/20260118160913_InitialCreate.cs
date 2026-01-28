using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bai3.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GiangVien",
                columns: table => new
                {
                    MaGiangVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenGiangVien = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    HocVi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChuyenNganh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiangVien", x => x.MaGiangVien);
                });

            migrationBuilder.CreateTable(
                name: "SinhVien",
                columns: table => new
                {
                    MaSinhVien = table.Column<int>(type: "int", nullable: false, comment: "Mã sinh viên")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoVaTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Họ và tên"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true, comment: "Số điện thoại"),
                    NgaySinh = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhVien", x => x.MaSinhVien);
                    table.CheckConstraint("CK_SinhVien_NgaySinh", "[NgaySinh] <= GETDATE()");
                });

            migrationBuilder.CreateTable(
                name: "KhoaHoc",
                columns: table => new
                {
                    MaKhoaHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKhoaHoc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "date", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "date", nullable: true),
                    SoTinChi = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    HocPhi = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    MaGiangVien = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhoaHoc", x => x.MaKhoaHoc);
                    table.CheckConstraint("CK_KhoaHoc_Ngay", "[NgayKetThuc] IS NULL OR [NgayKetThuc] >= [NgayBatDau]");
                    table.CheckConstraint("CK_KhoaHoc_SoTinChi", "[SoTinChi] BETWEEN 1 AND 10");
                    table.ForeignKey(
                        name: "FK_KhoaHoc_GiangVien",
                        column: x => x.MaGiangVien,
                        principalTable: "GiangVien",
                        principalColumn: "MaGiangVien",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DangKyKhoaHoc",
                columns: table => new
                {
                    MaSinhVien = table.Column<int>(type: "int", nullable: false),
                    MaKhoaHoc = table.Column<int>(type: "int", nullable: false),
                    NgayDangKy = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    TrangThaiDangKy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Đã đăng ký")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKyKhoaHoc", x => new { x.MaSinhVien, x.MaKhoaHoc });
                    table.CheckConstraint("CK_DangKyKhoaHoc_TrangThai", "[TrangThaiDangKy] IN (N'Đã đăng ký', N'Đã hủy', N'Chờ duyệt')");
                    table.ForeignKey(
                        name: "FK_DangKyKhoaHoc_KhoaHoc",
                        column: x => x.MaKhoaHoc,
                        principalTable: "KhoaHoc",
                        principalColumn: "MaKhoaHoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DangKyKhoaHoc_SinhVien",
                        column: x => x.MaSinhVien,
                        principalTable: "SinhVien",
                        principalColumn: "MaSinhVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThanhToan",
                columns: table => new
                {
                    MaThanhToan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSinhVien = table.Column<int>(type: "int", nullable: false),
                    MaKhoaHoc = table.Column<int>(type: "int", nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Chưa thanh toán"),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToan", x => x.MaThanhToan);
                    table.UniqueConstraint("AK_ThanhToan_UniquePayment", x => new { x.MaSinhVien, x.MaKhoaHoc, x.NgayThanhToan });
                    table.CheckConstraint("CK_ThanhToan_Ngay", "[NgayThanhToan] <= GETDATE()");
                    table.CheckConstraint("CK_ThanhToan_SoTien", "[SoTien] >= 0");
                    table.CheckConstraint("CK_ThanhToan_TrangThai", "[TrangThai] IN (N'Đã thanh toán', N'Chưa thanh toán', N'Đã hủy')");
                    table.ForeignKey(
                        name: "FK_ThanhToan_KhoaHoc",
                        column: x => x.MaKhoaHoc,
                        principalTable: "KhoaHoc",
                        principalColumn: "MaKhoaHoc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThanhToan_SinhVien",
                        column: x => x.MaSinhVien,
                        principalTable: "SinhVien",
                        principalColumn: "MaSinhVien",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "GiangVien",
                columns: new[] { "MaGiangVien", "ChuyenNganh", "Email", "HocVi", "SoDienThoai", "TenGiangVien" },
                values: new object[,]
                {
                    { 1, "Toán học", "nguyenvana@university.edu", "Tiến sĩ", "0912345678", "TS. Nguyễn Văn A" },
                    { 2, "Công nghệ thông tin", "tranthib@university.edu", "Thạc sĩ", "0923456789", "ThS. Trần Thị B" }
                });

            migrationBuilder.InsertData(
                table: "SinhVien",
                columns: new[] { "MaSinhVien", "Email", "HoVaTen", "NgaySinh", "SoDienThoai" },
                values: new object[,]
                {
                    { 1, "an.nguyen@student.edu", "Nguyễn Văn An", new DateTime(2000, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "0934567890" },
                    { 2, "binh.tran@student.edu", "Trần Thị Bình", new DateTime(2001, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "0945678901" },
                    { 3, "cuong.le@student.edu", "Lê Văn Cường", new DateTime(1999, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "KhoaHoc",
                columns: new[] { "MaKhoaHoc", "HocPhi", "MaGiangVien", "NgayBatDau", "NgayKetThuc", "SoTinChi", "TenKhoaHoc" },
                values: new object[,]
                {
                    { 1, 1500000m, 1, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Toán cao cấp A1" },
                    { 2, 2000000m, 2, new DateTime(2024, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Lập trình C# nâng cao" },
                    { 3, 1800000m, 2, new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, "Cơ sở dữ liệu" }
                });

            migrationBuilder.InsertData(
                table: "DangKyKhoaHoc",
                columns: new[] { "MaKhoaHoc", "MaSinhVien", "NgayDangKy", "TrangThaiDangKy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đã đăng ký" },
                    { 2, 1, new DateTime(2024, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đã đăng ký" },
                    { 2, 2, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đã đăng ký" },
                    { 3, 2, new DateTime(2024, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đã đăng ký" },
                    { 1, 3, new DateTime(2024, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đã đăng ký" }
                });

            migrationBuilder.InsertData(
                table: "ThanhToan",
                columns: new[] { "MaThanhToan", "GhiChu", "MaKhoaHoc", "MaSinhVien", "NgayThanhToan", "PhuongThucThanhToan", "SoTien", "TrangThai" },
                values: new object[,]
                {
                    { 1, null, 1, 1, new DateTime(2024, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chuyển khoản", 1500000m, "Đã thanh toán" },
                    { 2, null, 2, 1, new DateTime(2024, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tiền mặt", 1000000m, "Đã thanh toán" },
                    { 3, "Thanh toán đủ", 2, 2, new DateTime(2024, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2000000m, "Đã thanh toán" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DangKyKhoaHoc_MaKhoaHoc",
                table: "DangKyKhoaHoc",
                column: "MaKhoaHoc");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyKhoaHoc_NgayDangKy",
                table: "DangKyKhoaHoc",
                column: "NgayDangKy");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyKhoaHoc_TrangThai",
                table: "DangKyKhoaHoc",
                column: "TrangThaiDangKy");

            migrationBuilder.CreateIndex(
                name: "UQ_DangKyKhoaHoc_SinhVien_KhoaHoc",
                table: "DangKyKhoaHoc",
                columns: new[] { "MaSinhVien", "MaKhoaHoc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiangVien_Email",
                table: "GiangVien",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiangVien_SoDienThoai",
                table: "GiangVien",
                column: "SoDienThoai",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KhoaHoc_MaGiangVien",
                table: "KhoaHoc",
                column: "MaGiangVien");

            migrationBuilder.CreateIndex(
                name: "IX_KhoaHoc_NgayBatDau",
                table: "KhoaHoc",
                column: "NgayBatDau");

            migrationBuilder.CreateIndex(
                name: "IX_KhoaHoc_TenKhoaHoc",
                table: "KhoaHoc",
                column: "TenKhoaHoc");

            migrationBuilder.CreateIndex(
                name: "IX_SinhVien_Email",
                table: "SinhVien",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SinhVien_HoVaTen",
                table: "SinhVien",
                column: "HoVaTen");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_MaKhoaHoc",
                table: "ThanhToan",
                column: "MaKhoaHoc");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_NgayThanhToan",
                table: "ThanhToan",
                column: "NgayThanhToan");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_SinhVien_KhoaHoc",
                table: "ThanhToan",
                columns: new[] { "MaSinhVien", "MaKhoaHoc" });

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_TrangThai",
                table: "ThanhToan",
                column: "TrangThai");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DangKyKhoaHoc");

            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropTable(
                name: "KhoaHoc");

            migrationBuilder.DropTable(
                name: "SinhVien");

            migrationBuilder.DropTable(
                name: "GiangVien");
        }
    }
}
