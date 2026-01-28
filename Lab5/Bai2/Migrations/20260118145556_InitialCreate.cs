using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bai2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NGUOIQULYs",
                columns: table => new
                {
                    NguoiQuanLy_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenQuanLy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NGUOIQULYs", x => x.NguoiQuanLy_id);
                });

            migrationBuilder.CreateTable(
                name: "PHONGBANs",
                columns: table => new
                {
                    Phongban_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHONGBANs", x => x.Phongban_id);
                });

            migrationBuilder.CreateTable(
                name: "NHANVIENs",
                columns: table => new
                {
                    Nhanvien_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nhanvien_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: false),
                    Luong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PHG = table.Column<int>(type: "int", nullable: true),
                    Ma_NQL = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHANVIENs", x => x.Nhanvien_id);
                    table.ForeignKey(
                        name: "FK_NHANVIEN_NGUOIQULY",
                        column: x => x.Ma_NQL,
                        principalTable: "NGUOIQULYs",
                        principalColumn: "NguoiQuanLy_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_NHANVIEN_PHONGBAN",
                        column: x => x.PHG,
                        principalTable: "PHONGBANs",
                        principalColumn: "Phongban_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "THANNHANs",
                columns: table => new
                {
                    ThanNhan_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenThanNhan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuanHe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nhanvien_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THANNHANs", x => x.ThanNhan_id);
                    table.ForeignKey(
                        name: "FK_THANNHAN_NHANVIEN",
                        column: x => x.Nhanvien_id,
                        principalTable: "NHANVIENs",
                        principalColumn: "Nhanvien_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NHANVIENs_Ma_NQL",
                table: "NHANVIENs",
                column: "Ma_NQL");

            migrationBuilder.CreateIndex(
                name: "IX_NHANVIENs_PHG",
                table: "NHANVIENs",
                column: "PHG");

            migrationBuilder.CreateIndex(
                name: "IX_THANNHANs_Nhanvien_id",
                table: "THANNHANs",
                column: "Nhanvien_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "THANNHANs");

            migrationBuilder.DropTable(
                name: "NHANVIENs");

            migrationBuilder.DropTable(
                name: "NGUOIQULYs");

            migrationBuilder.DropTable(
                name: "PHONGBANs");
        }
    }
}
