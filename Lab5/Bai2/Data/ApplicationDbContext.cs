using Microsoft.EntityFrameworkCore;
using Bai2.Models;

namespace Bai2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Thêm constructor không tham số cho design-time
        protected ApplicationDbContext()
        {
        }

        public DbSet<NHANVIEN> NHANVIENs { get; set; }
        public DbSet<THANNHAN> THANNHANs { get; set; }
        public DbSet<PHONGBAN> PHONGBANs { get; set; }
        public DbSet<NGUOIQULY> NGUOIQULYs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Cấu hình NHANVIEN ---
            modelBuilder.Entity<NHANVIEN>(entity =>
            {
                entity.HasKey(e => e.Nhanvien_id);
                entity.Property(e => e.Nhanvien_name)
                      .HasMaxLength(100)
                      .IsRequired();
                entity.Property(e => e.DiaChi)
                      .HasMaxLength(200);
                entity.Property(e => e.Luong)
                      .HasColumnType("decimal(18,2)");

                // Quan hệ với PHONGBAN
                entity.HasOne(e => e.PhongBan)
                      .WithMany(p => p.NhanViens)
                      .HasForeignKey(e => e.PHG)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_NHANVIEN_PHONGBAN");

                // Quan hệ với NGUOIQULY
                entity.HasOne(e => e.NguoiQuanLy)
                      .WithMany(q => q.NhanViens)
                      .HasForeignKey(e => e.Ma_NQL)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_NHANVIEN_NGUOIQULY");
            });

            // --- Cấu hình THANNHAN ---
            modelBuilder.Entity<THANNHAN>(entity =>
            {
                entity.HasKey(e => e.ThanNhan_id);
                entity.Property(e => e.TenThanNhan)
                      .HasMaxLength(100)
                      .IsRequired();
                entity.Property(e => e.QuanHe)
                      .HasMaxLength(50);

                // Quan hệ với NHANVIEN
                entity.HasOne(e => e.NhanVien)
                      .WithMany(nv => nv.ThanNhans)
                      .HasForeignKey(e => e.Nhanvien_id)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_THANNHAN_NHANVIEN");
            });

            // --- Cấu hình PHONGBAN ---
            modelBuilder.Entity<PHONGBAN>(entity =>
            {
                entity.HasKey(e => e.Phongban_id);
                entity.Property(e => e.TenPhong)
                      .HasMaxLength(100)
                      .IsRequired();
            });

            // --- Cấu hình NGUOIQULY ---
            modelBuilder.Entity<NGUOIQULY>(entity =>
            {
                entity.HasKey(e => e.NguoiQuanLy_id);
                entity.Property(e => e.TenQuanLy)
                      .HasMaxLength(100)
                      .IsRequired();
            });
        }
    }
}