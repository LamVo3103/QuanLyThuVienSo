using System;
using System.Collections.Generic;

namespace QuanLyThuVienSo.API.Models;

public partial class PhieuMuon
{
    public int MaPhieu { get; set; }

    public string? MaDocGia { get; set; }

    public DateTime? NgayMuon { get; set; }

    public DateTime? NgayTraDuKien { get; set; }

    // Đây là cột mới thêm
    public DateTime? NgayTraThucTe { get; set; }

    public virtual ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();

    public virtual DocGia? MaDocGiaNavigation { get; set; }
}