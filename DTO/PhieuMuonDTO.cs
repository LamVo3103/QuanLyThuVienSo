namespace QuanLyThuVienSo.API.DTO
{
    public class TaoPhieuRequest
    {
        public string MaDocGia { get; set; } = null!;
        public List<SachMuonItem> DanhSachSachMuon { get; set; } = new();
    }

    public class SachMuonItem
    {
        public string MaSach { get; set; } = null!;
        public int SoLuong { get; set; }
    }
}