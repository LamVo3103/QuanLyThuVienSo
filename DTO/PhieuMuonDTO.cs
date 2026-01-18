namespace QuanLyThuVienSo.API.DTO
{
    // Class nhận dữ liệu từ Client gửi lên để tạo phiếu
    public class TaoPhieuMuonDTO
    {
        public string MaDocGia { get; set; } = null!;
        public DateTime? NgayTraDuKien { get; set; }
        public List<ChiTietMuonDTO> DanhSachSachMuon { get; set; } = new();
    }

    public class ChiTietMuonDTO 
    {
        public string MaSach { get; set; } = null!;
        public int SoLuong { get; set; }
    }

    // Class trả dữ liệu về Client để hiển thị danh sách đang mượn
    public class PhieuMuonHienThiDTO
    {
        public int MaPhieu { get; set; }
        public string HoTen { get; set; } = ""; // Tên độc giả
        public DateTime NgayMuon { get; set; }
        public DateTime NgayTraDuKien { get; set; }
        public string TenSach { get; set; } = ""; 
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } = "";
        public decimal TienPhat { get; set; } 
    }
}