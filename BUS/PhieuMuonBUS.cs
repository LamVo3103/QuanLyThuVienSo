using QuanLyThuVienSo.API.DAL;
using QuanLyThuVienSo.API.DTO;
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.BUS
{
    public class PhieuMuonBUS
    {
        private readonly PhieuMuonDAL _dal;
        private readonly DocGiaDAL _docGiaDAL; // Cần kiểm tra độc giả
        private readonly SachDAL _sachDAL;     // Cần kiểm tra sách

        public PhieuMuonBUS(PhieuMuonDAL dal, DocGiaDAL docGiaDAL, SachDAL sachDAL)
        {
            _dal = dal; _docGiaDAL = docGiaDAL; _sachDAL = sachDAL;
        }

        // CHỨC NĂNG MƯỢN SÁCH
        public async Task<int> MuonSach(string maDocGia, List<SachMuonItem> items)
        {
            if (!await _docGiaDAL.Exists(maDocGia)) throw new Exception("Độc giả không tồn tại");

            using var transaction = _dal.Context.Database.BeginTransaction();
            try
            {
                var phieu = new PhieuMuon { MaDocGia = maDocGia, NgayMuon = DateTime.Now, NgayTraDuKien = DateTime.Now.AddDays(7) };
                _dal.Context.PhieuMuons.Add(phieu);
                await _dal.Context.SaveChangesAsync(); // Lưu để lấy ID

                foreach (var item in items)
                {
                    var sach = await _sachDAL.GetById(item.MaSach);
                    if (sach == null) throw new Exception($"Sách {item.MaSach} lỗi");
                    if (sach.SoLuong < item.SoLuong) throw new Exception($"Sách {sach.TenSach} hết hàng");

                    _dal.Context.ChiTietPhieuMuons.Add(new ChiTietPhieuMuon 
                    { 
                        MaPhieu = phieu.MaPhieu, MaSach = item.MaSach, SoLuong = item.SoLuong, DonGia = sach.GiaTien 
                    });
                    sach.SoLuong -= item.SoLuong;
                }
                await _dal.Context.SaveChangesAsync();
                await transaction.CommitAsync();
                return phieu.MaPhieu;
            }
            catch { await transaction.RollbackAsync(); throw; }
        }

        // CHỨC NĂNG TRẢ SÁCH
        public async Task TraSach(int maPhieu)
        {
            var phieu = await _dal.GetById(maPhieu);
            if (phieu == null) throw new Exception("Không tìm thấy phiếu");
            if (phieu.NgayTraThucTe != null) throw new Exception("Phiếu đã trả rồi");

            phieu.NgayTraThucTe = DateTime.Now;

            foreach (var ct in phieu.ChiTietPhieuMuons)
            {
                var sach = await _sachDAL.GetById(ct.MaSach);
                if (sach != null) sach.SoLuong += ct.SoLuong;
            }
            await _dal.Context.SaveChangesAsync();
        }
    }
}