using System;
using System.Collections.Generic;

namespace QuanLyThuVienSo.API.Models;

public partial class ChiTietPhieuMuon
{
    public int MaPhieu { get; set; }

    public string MaSach { get; set; } = null!;

    public int? SoLuong { get; set; }

    public decimal? DonGia { get; set; }

    public virtual PhieuMuon MaPhieuNavigation { get; set; } = null!;

    public virtual Sach MaSachNavigation { get; set; } = null!;
}