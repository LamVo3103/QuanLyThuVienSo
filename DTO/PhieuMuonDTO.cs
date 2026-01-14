namespace QuanLyThuVienSo.API.DTO
{
    public class TaoPhieuMuonDTO
    {
        public string MaDocGia { get; set; } = null!;
        public DateTime? NgayTraDuKien { get; set; }
        
        // Tên class trong List<> này phải khớp với tham số bên BUS
        public List<ChiTietMuonDTO> DanhSachSachMuon { get; set; } = new();
    }

    // Đây là class mà BUS đang gọi
    public class ChiTietMuonDTO 
    {
        public string MaSach { get; set; } = null!;
        public int SoLuong { get; set; }
    }
}