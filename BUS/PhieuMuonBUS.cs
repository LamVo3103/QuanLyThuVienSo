using QuanLyThuVienSo.API.DAL;
using QuanLyThuVienSo.API.DTO; // QUAN TRỌNG: Phải có dòng này để dùng DTO
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.BUS
{
    public class PhieuMuonBUS
    {
        private readonly PhieuMuonDAL _dal;
        private readonly DocGiaDAL _docGiaDAL;
        private readonly SachDAL _sachDAL;

        public PhieuMuonBUS(PhieuMuonDAL dal, DocGiaDAL docGiaDAL, SachDAL sachDAL)
        {
            _dal = dal; 
            _docGiaDAL = docGiaDAL; 
            _sachDAL = sachDAL;
        }

        // CHỨC NĂNG MƯỢN SÁCH
        // Sửa lỗi ở đây: Đổi List<SachMuonItem> thành List<ChiTietMuonDTO>
        public async Task<int> MuonSach(string maDocGia, List<ChiTietMuonDTO> items)
        {
            // 1. Kiểm tra độc giả có tồn tại không
            if (!await _docGiaDAL.Exists(maDocGia)) 
                throw new Exception("Độc giả không tồn tại");

            using var transaction = _dal.Context.Database.BeginTransaction();
            try
            {
                // 2. Tạo Phiếu Mượn (Cha)
                var phieu = new PhieuMuon 
                { 
                    MaDocGia = maDocGia, 
                    NgayMuon = DateTime.Now, 
                    NgayTraDuKien = DateTime.Now.AddDays(7) // Mặc định mượn 7 ngày
                };
                
                _dal.Context.PhieuMuons.Add(phieu);
                await _dal.Context.SaveChangesAsync(); // Lưu để lấy được MaPhieu tự sinh

                // 3. Tạo Chi Tiết Phiếu Mượn (Con)
                foreach (var item in items)
                {
                    // Kiểm tra sách
                    var sach = await _sachDAL.GetById(item.MaSach);
                    if (sach == null) throw new Exception($"Mã sách {item.MaSach} không tồn tại");
                    if (sach.SoLuong < item.SoLuong) throw new Exception($"Sách '{sach.TenSach}' không đủ số lượng");

                    // Tạo chi tiết
                    _dal.Context.ChiTietPhieuMuons.Add(new ChiTietPhieuMuon 
                    { 
                        MaPhieu = phieu.MaPhieu, // Lấy ID vừa tạo ở trên
                        MaSach = item.MaSach, 
                        SoLuong = item.SoLuong, 
                        DonGia = sach.GiaTien 
                    });

                    // Trừ kho
                    sach.SoLuong -= item.SoLuong;
                }

                await _dal.Context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                return phieu.MaPhieu; // Trả về Mã phiếu để báo cho Frontend
            }
            catch 
            { 
                await transaction.RollbackAsync(); 
                throw; 
            }
        }

        // CHỨC NĂNG TRẢ SÁCH (Giữ nguyên)
        public async Task TraSach(int maPhieu)
        {
            var phieu = await _dal.GetById(maPhieu);
            if (phieu == null) throw new Exception("Không tìm thấy phiếu mượn");
            if (phieu.NgayTraThucTe != null) throw new Exception("Phiếu này đã trả rồi");

            // Cập nhật ngày trả
            phieu.NgayTraThucTe = DateTime.Now;

            // Cộng lại số lượng sách vào kho
            foreach (var ct in phieu.ChiTietPhieuMuons)
            {
                var sach = await _sachDAL.GetById(ct.MaSach);
                if (sach != null) 
                {
                    sach.SoLuong += ct.SoLuong;
                }
            }
            await _dal.Context.SaveChangesAsync();
        }
    }
}