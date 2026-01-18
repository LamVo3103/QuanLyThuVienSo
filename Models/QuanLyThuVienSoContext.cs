using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuanLyThuVienSo.API.Models;

public partial class QuanLyThuVienSoContext : DbContext
{
    public QuanLyThuVienSoContext()
    {
    }

    public QuanLyThuVienSoContext(DbContextOptions<QuanLyThuVienSoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; }
    public virtual DbSet<DocGia> DocGias { get; set; } 
    public virtual DbSet<PhieuMuon> PhieuMuons { get; set; }
    public virtual DbSet<Sach> Saches { get; set; }
    public virtual DbSet<TacGia> TacGias { get; set; }
    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Chi Tiết Phiếu Mượn
        modelBuilder.Entity<ChiTietPhieuMuon>(entity =>
        {
            entity.HasKey(e => new { e.MaPhieu, e.MaSach }).HasName("chitietphieumuon_pkey");
            entity.ToTable("chitietphieumuon");
            entity.Property(e => e.MaPhieu).HasColumnName("maphieu");
            entity.Property(e => e.MaSach).HasMaxLength(20).HasColumnName("masach");
            entity.Property(e => e.DonGia).HasPrecision(18, 2).HasColumnName("dongia");
            entity.Property(e => e.SoLuong).HasColumnName("soluong");

            // MaPhieuNavigation -> PhieuMuon
            entity.HasOne(d => d.PhieuMuon) 
                .WithMany(p => p.ChiTietPhieuMuons)
                .HasForeignKey(d => d.MaPhieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ctpm_phieumuon");

            // MaSachNavigation -> Sach
            entity.HasOne(d => d.Sach) 
                .WithMany(p => p.ChiTietPhieuMuons)
                .HasForeignKey(d => d.MaSach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ctpm_sach");
        });

        // 2. Độc Giả
        modelBuilder.Entity<DocGia>(entity =>
        {
            entity.HasKey(e => e.MaDocGia).HasName("docgia_pkey");
            entity.ToTable("docgia");
            entity.HasIndex(e => e.Cccd, "docgia_cccd_key").IsUnique();
            entity.Property(e => e.MaDocGia).HasMaxLength(20).HasColumnName("madocgia");
            entity.Property(e => e.Cccd).HasMaxLength(20).HasColumnName("cccd");
            entity.Property(e => e.DiaChi).HasMaxLength(200).HasColumnName("diachi");
            entity.Property(e => e.DienThoai).HasMaxLength(15).HasColumnName("dienthoai");
            entity.Property(e => e.GioiTinh).HasMaxLength(10).HasColumnName("gioitinh");
            entity.Property(e => e.HoTen).HasMaxLength(100).HasColumnName("hoten");
            entity.Property(e => e.NgayLamThe).HasColumnType("timestamp without time zone").HasColumnName("ngaylamthe");
            entity.Property(e => e.NgaySinh).HasColumnType("timestamp without time zone").HasColumnName("ngaysinh");
        });

        // 3. Phiếu Mượn
        modelBuilder.Entity<PhieuMuon>(entity =>
        {
            entity.HasKey(e => e.MaPhieu).HasName("phieumuon_pkey");
            entity.ToTable("phieumuon");
            entity.Property(e => e.MaPhieu).HasColumnName("maphieu");
            entity.Property(e => e.MaDocGia).HasMaxLength(20).HasColumnName("madocgia");
            entity.Property(e => e.NgayMuon).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp without time zone").HasColumnName("ngaymuon");
            entity.Property(e => e.NgayTraDuKien).HasColumnType("timestamp without time zone").HasColumnName("ngaytradukien");
            entity.Property(e => e.NgayTraThucTe).HasColumnType("timestamp without time zone").HasColumnName("ngaytrathucte");

            // 👇 THÊM ĐOẠN NÀY ĐỂ HẾT LỖI "Column does not exist"
            entity.Property(e => e.TienPhat)
                .HasPrecision(18, 2)       // Định dạng số tiền
                .HasDefaultValue(0)        // Mặc định là 0
                .HasColumnName("tienphat"); // Map với cột chữ thường trong DB

            // MaDocGiaNavigation -> DocGia
            entity.HasOne(d => d.DocGia)
                .WithMany(p => p.PhieuMuons)
                .HasForeignKey(d => d.MaDocGia)
                .HasConstraintName("fk_phieumuon_docgia");
        });

        // 4. Sách
        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.MaSach).HasName("sach_pkey");
            entity.ToTable("sach");
            entity.Property(e => e.MaSach).HasMaxLength(20).HasColumnName("masach");
            entity.Property(e => e.GiaTien).HasPrecision(18, 2).HasColumnName("giatien");
            entity.Property(e => e.MaTacGia).HasMaxLength(20).HasColumnName("matacgia");
            entity.Property(e => e.NgayXuatBan).HasColumnType("timestamp without time zone").HasColumnName("ngayxuatban");
            entity.Property(e => e.NhaXuatBan).HasMaxLength(100).HasColumnName("nhaxuatban");
            entity.Property(e => e.SoLuong).HasDefaultValue(0).HasColumnName("soluong");
            entity.Property(e => e.TenSach).HasMaxLength(200).HasColumnName("tensach");
            entity.Property(e => e.TheLoai).HasMaxLength(100).HasColumnName("theloai");

            // Lưu ý: Nếu Model Sách chưa sửa tên navigation thì giữ nguyên MaTacGiaNavigation
            entity.HasOne(d => d.MaTacGiaNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaTacGia).HasConstraintName("fk_sach_tacgia");
        });

        // 5. Tác Giả
        modelBuilder.Entity<TacGia>(entity =>
        {
            entity.HasKey(e => e.MaTacGia).HasName("tacgia_pkey");
            entity.ToTable("tacgia");
            entity.Property(e => e.MaTacGia).HasMaxLength(20).HasColumnName("matacgia");
            entity.Property(e => e.GioiTinh).HasMaxLength(10).HasColumnName("gioitinh");
            entity.Property(e => e.HoTen).HasMaxLength(100).HasColumnName("hoten");
            entity.Property(e => e.QueQuan).HasMaxLength(200).HasColumnName("quequan");
        });

        // 6. Tài Khoản
        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("taikhoan_pkey");
            entity.ToTable("taikhoan");
            entity.Property(e => e.Username).HasMaxLength(50).HasColumnName("username");
            entity.Property(e => e.Password).HasMaxLength(255).HasColumnName("password");
            entity.Property(e => e.Role).HasMaxLength(20).HasDefaultValueSql("'User'::character varying").HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}