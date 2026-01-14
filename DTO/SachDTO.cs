namespace QuanLyThuVienSo.API.DTO
{
    public class SachDTO
    {
        public string MaSach { get; set; } = null!;
        public string TenSach { get; set; } = null!;
        public string? TheLoai { get; set; }
        public string? NhaXuatBan { get; set; }
        public DateTime? NgayXuatBan { get; set; }
        public string MaTacGia { get; set; } = null!; // Chỉ giữ lại Mã, bỏ object Tác giả
        public int? SoLuong { get; set; }
        public decimal? GiaTien { get; set; }
    }
}