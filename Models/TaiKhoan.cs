using System;
using System.Collections.Generic;

namespace QuanLyThuVienSo.API.Models;

public partial class TaiKhoan
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Role { get; set; }
}
