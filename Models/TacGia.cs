using System;
using System.Collections.Generic;

namespace QuanLyThuVienSo.API.Models;

public partial class TacGia
{
    public string MaTacGia { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string? GioiTinh { get; set; }

    public string? QueQuan { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}