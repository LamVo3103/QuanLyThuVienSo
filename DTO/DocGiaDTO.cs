namespace QuanLyThuVienSo.API.DTO
{
    public class DocGiaDTO
    {
        public string MaDocGia { get; set; } = null!;
        public string HoTen { get; set; } = null!;
        public string GioiTinh { get; set; } = null!;
        public DateTime NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public string? DienThoai { get; set; }
        public string? Cccd { get; set; }
        public string? TrangThaiMuon { get; set; }
        public decimal TongTienPhat { get; set; }
    }
}