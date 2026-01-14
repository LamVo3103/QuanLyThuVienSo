namespace QuanLyThuVienSo.API.DTO
{
    public class TacGiaDTO
    {
        public string MaTacGia { get; set; } = null!;
        public string HoTen { get; set; } = null!;
        public string? GioiTinh { get; set; }
        public string? QueQuan { get; set; }
    }
}