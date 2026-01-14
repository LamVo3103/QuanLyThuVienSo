using System;
using System.Collections.Generic;

namespace QuanLyThuVienSo.API.Models;

public partial class DocGia
{
    public string MaDocGia { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string? GioiTinh { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? DiaChi { get; set; } // Đã sửa lỗi chính tả Diahi -> DiaChi

    public string? DienThoai { get; set; }

    public string? Cccd { get; set; }

    public DateTime? NgayLamThe { get; set; }

    public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();
}