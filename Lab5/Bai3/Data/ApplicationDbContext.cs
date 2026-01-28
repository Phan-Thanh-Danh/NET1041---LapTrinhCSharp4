using Microsoft.EntityFrameworkCore;
using Bai3.Models;

namespace Bai3.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<KhoaHoc> KhoaHocs { get; set; }
        public DbSet<GiangVien> GiangViens { get; set; }
        public DbSet<ThanhToan> ThanhToans { get; set; }
        public DbSet<DangKyKhoaHoc> DangKyKhoaHocs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==================== SINHVIEN ====================
            modelBuilder.Entity<SinhVien>(entity =>
            {
                // Table name
                entity.ToTable("SinhVien");

                // Primary key
                entity.HasKey(e => e.MaSinhVien);

                // Identity column
                entity.Property(e => e.MaSinhVien)
                      .ValueGeneratedOnAdd()
                      .HasColumnName("MaSinhVien")
                      .HasComment("Mã sinh viên");

                // Properties
                entity.Property(e => e.HoVaTen)
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasColumnName("HoVaTen")
                      .HasComment("Họ và tên");

                entity.Property(e => e.Email)
                      .HasMaxLength(100)
                      .HasColumnName("Email");

                entity.Property(e => e.SoDienThoai)
                      .HasMaxLength(15)
                      .HasColumnName("SoDienThoai")
                      .HasComment("Số điện thoại");

                entity.Property(e => e.NgaySinh)
                      .HasColumnType("date")
                      .HasColumnName("NgaySinh");

                // Indexes
                entity.HasIndex(e => e.Email)
                      .IsUnique()
                      .HasDatabaseName("IX_SinhVien_Email")
                      .HasFilter("[Email] IS NOT NULL");

                entity.HasIndex(e => e.HoVaTen)
                      .HasDatabaseName("IX_SinhVien_HoVaTen");

                // Check constraints
                entity.HasCheckConstraint("CK_SinhVien_NgaySinh", 
                    "[NgaySinh] <= GETDATE()");
            });

            // ==================== GIANGVIEN ====================
            modelBuilder.Entity<GiangVien>(entity =>
            {
                entity.ToTable("GiangVien");
                entity.HasKey(e => e.MaGiangVien);

                entity.Property(e => e.MaGiangVien)
                      .ValueGeneratedOnAdd()
                      .HasColumnName("MaGiangVien");

                entity.Property(e => e.TenGiangVien)
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasColumnName("TenGiangVien");

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasColumnName("Email");

                entity.Property(e => e.SoDienThoai)
                      .IsRequired()
                      .HasMaxLength(15)
                      .HasColumnName("SoDienThoai");

                entity.Property(e => e.HocVi)
                      .HasMaxLength(50)
                      .HasColumnName("HocVi");

                entity.Property(e => e.ChuyenNganh)
                      .HasMaxLength(100)
                      .HasColumnName("ChuyenNganh");

                // Index
                entity.HasIndex(e => e.Email)
                      .IsUnique()
                      .HasDatabaseName("IX_GiangVien_Email");

                entity.HasIndex(e => e.SoDienThoai)
                      .IsUnique()
                      .HasDatabaseName("IX_GiangVien_SoDienThoai");
            });

            // ==================== KHOAHOC ====================
            modelBuilder.Entity<KhoaHoc>(entity =>
            {
                entity.ToTable("KhoaHoc");
                entity.HasKey(e => e.MaKhoaHoc);

                entity.Property(e => e.MaKhoaHoc)
                      .ValueGeneratedOnAdd()
                      .HasColumnName("MaKhoaHoc");

                entity.Property(e => e.TenKhoaHoc)
                      .IsRequired()
                      .HasMaxLength(200)
                      .HasColumnName("TenKhoaHoc");

                entity.Property(e => e.NgayBatDau)
                      .IsRequired()
                      .HasColumnType("date")
                      .HasColumnName("NgayBatDau");

                entity.Property(e => e.NgayKetThuc)
                      .HasColumnType("date")
                      .HasColumnName("NgayKetThuc");

                entity.Property(e => e.SoTinChi)
                      .IsRequired()
                      .HasDefaultValue(3)
                      .HasColumnName("SoTinChi");

                entity.Property(e => e.HocPhi)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)")
                      .HasColumnName("HocPhi")
                      .HasDefaultValue(0);

                // Foreign Key to GiangVien
                entity.Property(e => e.MaGiangVien)
                      .HasColumnName("MaGiangVien");

                // Relationship with GiangVien
                entity.HasOne(e => e.GiangVien)
                      .WithMany(g => g.KhoaHocs)
                      .HasForeignKey(e => e.MaGiangVien)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_KhoaHoc_GiangVien");

                // Check constraint
                entity.HasCheckConstraint("CK_KhoaHoc_Ngay", 
                    "[NgayKetThuc] IS NULL OR [NgayKetThuc] >= [NgayBatDau]");

                entity.HasCheckConstraint("CK_KhoaHoc_SoTinChi", 
                    "[SoTinChi] BETWEEN 1 AND 10");

                // Index
                entity.HasIndex(e => e.TenKhoaHoc)
                      .HasDatabaseName("IX_KhoaHoc_TenKhoaHoc");

                entity.HasIndex(e => e.NgayBatDau)
                      .HasDatabaseName("IX_KhoaHoc_NgayBatDau");
            });

            // ==================== DANGKYKHOAHOC ====================
            modelBuilder.Entity<DangKyKhoaHoc>(entity =>
            {
                entity.ToTable("DangKyKhoaHoc");

                // Composite primary key
                entity.HasKey(e => new { e.MaSinhVien, e.MaKhoaHoc })
                      .HasName("PK_DangKyKhoaHoc");

                entity.Property(e => e.MaSinhVien)
                      .HasColumnName("MaSinhVien");

                entity.Property(e => e.MaKhoaHoc)
                      .HasColumnName("MaKhoaHoc");

                entity.Property(e => e.NgayDangKy)
                      .IsRequired()
                      .HasColumnType("datetime")
                      .HasColumnName("NgayDangKy")
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.TrangThaiDangKy)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasColumnName("TrangThaiDangKy")
                      .HasDefaultValue("Đã đăng ký");

                // Relationships
                entity.HasOne(e => e.SinhVien)
                      .WithMany(s => s.DangKyKhoaHocs)
                      .HasForeignKey(e => e.MaSinhVien)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_DangKyKhoaHoc_SinhVien");

                entity.HasOne(e => e.KhoaHoc)
                      .WithMany(k => k.DangKyKhoaHocs)
                      .HasForeignKey(e => e.MaKhoaHoc)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_DangKyKhoaHoc_KhoaHoc");

                // Index
                entity.HasIndex(e => e.NgayDangKy)
                      .HasDatabaseName("IX_DangKyKhoaHoc_NgayDangKy");

                entity.HasIndex(e => e.TrangThaiDangKy)
                      .HasDatabaseName("IX_DangKyKhoaHoc_TrangThai");

                // Check constraint - SỬA: Thêm N' cho Unicode
                entity.HasCheckConstraint("CK_DangKyKhoaHoc_TrangThai", 
                    "[TrangThaiDangKy] IN (N'Đã đăng ký', N'Đã hủy', N'Chờ duyệt')");
            });

            // ==================== THANHTOAN ====================
            modelBuilder.Entity<ThanhToan>(entity =>
            {
                entity.ToTable("ThanhToan");
                entity.HasKey(e => e.MaThanhToan);

                entity.Property(e => e.MaThanhToan)
                      .ValueGeneratedOnAdd()
                      .HasColumnName("MaThanhToan");

                entity.Property(e => e.MaSinhVien)
                      .IsRequired()
                      .HasColumnName("MaSinhVien");

                entity.Property(e => e.MaKhoaHoc)
                      .IsRequired()
                      .HasColumnName("MaKhoaHoc");

                entity.Property(e => e.SoTien)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)")
                      .HasColumnName("SoTien");

                entity.Property(e => e.NgayThanhToan)
                      .IsRequired()
                      .HasColumnType("datetime")
                      .HasColumnName("NgayThanhToan")
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.TrangThai)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasColumnName("TrangThai")
                      .HasDefaultValue("Chưa thanh toán");

                entity.Property(e => e.PhuongThucThanhToan)
                      .HasMaxLength(50)
                      .HasColumnName("PhuongThucThanhToan");

                entity.Property(e => e.GhiChu)
                      .HasMaxLength(500)
                      .HasColumnName("GhiChu");

                // Relationships (Restrict để tránh xóa nhầm)
                entity.HasOne(e => e.SinhVien)
                      .WithMany(s => s.ThanhToans)
                      .HasForeignKey(e => e.MaSinhVien)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_ThanhToan_SinhVien");

                entity.HasOne(e => e.KhoaHoc)
                      .WithMany(k => k.ThanhToans)
                      .HasForeignKey(e => e.MaKhoaHoc)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_ThanhToan_KhoaHoc");

                // Indexes
                entity.HasIndex(e => new { e.MaSinhVien, e.MaKhoaHoc })
                      .HasDatabaseName("IX_ThanhToan_SinhVien_KhoaHoc");

                entity.HasIndex(e => e.NgayThanhToan)
                      .HasDatabaseName("IX_ThanhToan_NgayThanhToan");

                entity.HasIndex(e => e.TrangThai)
                      .HasDatabaseName("IX_ThanhToan_TrangThai");

                // Check constraints
                entity.HasCheckConstraint("CK_ThanhToan_SoTien", 
                    "[SoTien] >= 0");

                // SỬA: Thêm N' cho Unicode
                entity.HasCheckConstraint("CK_ThanhToan_TrangThai", 
                    "[TrangThai] IN (N'Đã thanh toán', N'Chưa thanh toán', N'Đã hủy')");

                entity.HasCheckConstraint("CK_ThanhToan_Ngay", 
                    "[NgayThanhToan] <= GETDATE()");
            });

            // ==================== QUAN HỆ PHỨC TẠP ====================
            
            // Đảm bảo mỗi sinh viên chỉ đăng ký 1 lần cho 1 khóa học
            modelBuilder.Entity<DangKyKhoaHoc>()
                .HasIndex(e => new { e.MaSinhVien, e.MaKhoaHoc })
                .IsUnique()
                .HasDatabaseName("UQ_DangKyKhoaHoc_SinhVien_KhoaHoc");
            
            // Đảm bảo không có thanh toán trùng
            modelBuilder.Entity<ThanhToan>()
                .HasAlternateKey(e => new { e.MaSinhVien, e.MaKhoaHoc, e.NgayThanhToan })
                .HasName("AK_ThanhToan_UniquePayment");

            // ==================== SEED DATA ====================
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // 1. Giảng viên
            modelBuilder.Entity<GiangVien>().HasData(
                new GiangVien
                {
                    MaGiangVien = 1,
                    TenGiangVien = "TS. Nguyễn Văn A",
                    Email = "nguyenvana@university.edu",
                    SoDienThoai = "0912345678",
                    HocVi = "Tiến sĩ",
                    ChuyenNganh = "Toán học"
                },
                new GiangVien
                {
                    MaGiangVien = 2,
                    TenGiangVien = "ThS. Trần Thị B",
                    Email = "tranthib@university.edu",
                    SoDienThoai = "0923456789",
                    HocVi = "Thạc sĩ",
                    ChuyenNganh = "Công nghệ thông tin"
                }
            );

            // 2. Sinh viên
            modelBuilder.Entity<SinhVien>().HasData(
                new SinhVien
                {
                    MaSinhVien = 1,
                    HoVaTen = "Nguyễn Văn An",
                    Email = "an.nguyen@student.edu",
                    SoDienThoai = "0934567890",
                    NgaySinh = new DateTime(2000, 5, 15)
                },
                new SinhVien
                {
                    MaSinhVien = 2,
                    HoVaTen = "Trần Thị Bình",
                    Email = "binh.tran@student.edu",
                    SoDienThoai = "0945678901",
                    NgaySinh = new DateTime(2001, 8, 22)
                },
                new SinhVien
                {
                    MaSinhVien = 3,
                    HoVaTen = "Lê Văn Cường",
                    Email = "cuong.le@student.edu",
                    NgaySinh = new DateTime(1999, 11, 30)
                }
            );

            // 3. Khóa học
            modelBuilder.Entity<KhoaHoc>().HasData(
                new KhoaHoc
                {
                    MaKhoaHoc = 1,
                    TenKhoaHoc = "Toán cao cấp A1",
                    NgayBatDau = new DateTime(2024, 9, 1),
                    NgayKetThuc = new DateTime(2024, 12, 20),
                    SoTinChi = 3,
                    HocPhi = 1500000,
                    MaGiangVien = 1
                },
                new KhoaHoc
                {
                    MaKhoaHoc = 2,
                    TenKhoaHoc = "Lập trình C# nâng cao",
                    NgayBatDau = new DateTime(2024, 9, 15),
                    NgayKetThuc = new DateTime(2024, 12, 30),
                    SoTinChi = 4,
                    HocPhi = 2000000,
                    MaGiangVien = 2
                },
                new KhoaHoc
                {
                    MaKhoaHoc = 3,
                    TenKhoaHoc = "Cơ sở dữ liệu",
                    NgayBatDau = new DateTime(2024, 10, 1),
                    SoTinChi = 3,
                    HocPhi = 1800000,
                    MaGiangVien = 2
                }
            );

            // 4. Đăng ký khóa học
            modelBuilder.Entity<DangKyKhoaHoc>().HasData(
                new DangKyKhoaHoc
                {
                    MaSinhVien = 1,
                    MaKhoaHoc = 1,
                    NgayDangKy = new DateTime(2024, 8, 20),
                    TrangThaiDangKy = "Đã đăng ký"
                },
                new DangKyKhoaHoc
                {
                    MaSinhVien = 1,
                    MaKhoaHoc = 2,
                    NgayDangKy = new DateTime(2024, 8, 25),
                    TrangThaiDangKy = "Đã đăng ký"
                },
                new DangKyKhoaHoc
                {
                    MaSinhVien = 2,
                    MaKhoaHoc = 2,
                    NgayDangKy = new DateTime(2024, 9, 1),
                    TrangThaiDangKy = "Đã đăng ký"
                },
                new DangKyKhoaHoc
                {
                    MaSinhVien = 2,
                    MaKhoaHoc = 3,
                    NgayDangKy = new DateTime(2024, 9, 5),
                    TrangThaiDangKy = "Đã đăng ký"
                },
                new DangKyKhoaHoc
                {
                    MaSinhVien = 3,
                    MaKhoaHoc = 1,
                    NgayDangKy = new DateTime(2024, 8, 22),
                    TrangThaiDangKy = "Đã đăng ký"
                }
            );

            // 5. Thanh toán
            modelBuilder.Entity<ThanhToan>().HasData(
                new ThanhToan
                {
                    MaThanhToan = 1,
                    MaSinhVien = 1,
                    MaKhoaHoc = 1,
                    SoTien = 1500000,
                    NgayThanhToan = new DateTime(2024, 8, 20),
                    TrangThai = "Đã thanh toán",
                    PhuongThucThanhToan = "Chuyển khoản"
                },
                new ThanhToan
                {
                    MaThanhToan = 2,
                    MaSinhVien = 1,
                    MaKhoaHoc = 2,
                    SoTien = 1000000,
                    NgayThanhToan = new DateTime(2024, 9, 10),
                    TrangThai = "Đã thanh toán",
                    PhuongThucThanhToan = "Tiền mặt"
                },
                new ThanhToan
                {
                    MaThanhToan = 3,
                    MaSinhVien = 2,
                    MaKhoaHoc = 2,
                    SoTien = 2000000,
                    NgayThanhToan = new DateTime(2024, 9, 12),
                    TrangThai = "Đã thanh toán",
                    GhiChu = "Thanh toán đủ"
                }
            );
        }
    } 
}