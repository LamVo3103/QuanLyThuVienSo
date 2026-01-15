using System;
using System.Collections.Generic;

namespace QuanLyThuVienSo.API.Models;

public partial class Sach
{
    public string MaSach { get; set; } = null!;

    public string TenSach { get; set; } = null!;

    public string? TheLoai { get; set; }

    public string? NhaXuatBan { get; set; }

    public DateTime? NgayXuatBan { get; set; }

    public string? MaTacGia { get; set; }
    public int? SoLuong { get; set; }

    public decimal? GiaTien { get; set; }

    public virtual ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();

    public virtual TacGia? MaTacGiaNavigation { get; set; }

    public bool DangKinhDoanh { get; set; } = true;
}
