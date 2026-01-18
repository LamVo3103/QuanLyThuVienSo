using Microsoft.EntityFrameworkCore;
using QuanLyThuVienSo.API.DAL;
using QuanLyThuVienSo.API.DTO;
using QuanLyThuVienSo.API.Models;
using System.Linq; // Cần dòng này để dùng .Select, .Where, .Sum

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

        // 1. CHỨC NĂNG MƯỢN SÁCH
        public async Task<int> MuonSach(string maDocGia, List<ChiTietMuonDTO> items)
        {
            if (!await _docGiaDAL.Exists(maDocGia)) 
                throw new Exception("Độc giả không tồn tại");

            using var transaction = _dal.Context.Database.BeginTransaction();
            try
            {
                var phieu = new PhieuMuon 
                { 
                    MaDocGia = maDocGia, 
                    NgayMuon = DateTime.Now, 
                    NgayTraDuKien = DateTime.Now.AddDays(7) 
                };
                
                _dal.Context.PhieuMuons.Add(phieu);
                await _dal.Context.SaveChangesAsync(); 

                foreach (var item in items)
                {
                    var sach = await _sachDAL.GetById(item.MaSach);
                    if (sach == null) throw new Exception($"Mã sách {item.MaSach} không tồn tại");
                    if (sach.SoLuong < item.SoLuong) throw new Exception($"Sách '{sach.TenSach}' không đủ số lượng");

                    _dal.Context.ChiTietPhieuMuons.Add(new ChiTietPhieuMuon 
                    { 
                        MaPhieu = phieu.MaPhieu, 
                        MaSach = item.MaSach, 
                        SoLuong = item.SoLuong, 
                        DonGia = sach.GiaTien 
                    });

                    sach.SoLuong -= item.SoLuong;
                }

                await _dal.Context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                return phieu.MaPhieu; 
            }
            catch 
            { 
                await transaction.RollbackAsync(); 
                throw; 
            }
        }

        // 2. CHỨC NĂNG TRẢ SÁCH
        public async Task<(int soNgayTre, decimal tienPhat)> TraSach(int maPhieu)
        {
            int soNgayTre = 0;
            decimal tienPhat = 0;
            // 1. Lấy phiếu kèm chi tiết (để đếm số lượng sách)
            var phieu = await _dal.Context.PhieuMuons
                .Include(pm => pm.ChiTietPhieuMuons)
                .FirstOrDefaultAsync(pm => pm.MaPhieu == maPhieu);

            if (phieu == null) throw new Exception("Không tìm thấy phiếu mượn");
            if (phieu.NgayTraThucTe != null) throw new Exception("Phiếu này đã trả rồi");

            // 2. Cập nhật ngày trả
            DateTime ngayTraThucTe = DateTime.Now;
            phieu.NgayTraThucTe = ngayTraThucTe;

            // 3. --- TÍNH TIỀN PHẠT (LOGIC MỚI) ---
            string thongBaoPhat = "";
            
            // Nếu có ngày dự kiến và trả trễ hơn dự kiến
            if (phieu.NgayTraDuKien.HasValue && ngayTraThucTe.Date > phieu.NgayTraDuKien.Value.Date)
            {
                // Tính số ngày trễ (Làm tròn lên)
                TimeSpan tre = ngayTraThucTe.Date - phieu.NgayTraDuKien.Value.Date;
                soNgayTre = (int)tre.TotalDays;

                // Tính tổng số cuốn sách trong phiếu này
                int tongSoSach = phieu.ChiTietPhieuMuons.Sum(ct => ct.SoLuong);

                // Công thức: 20k * Số ngày * Số sách
                tienPhat = 20000 * soNgayTre * tongSoSach;
                
                phieu.TienPhat = tienPhat; // Lưu vào DB
                
                thongBaoPhat = $" (Đã quá hạn {soNgayTre} ngày. Phạt: {tienPhat:N0} VNĐ)";
            }

            // 4. Cộng lại kho sách (như cũ)
            foreach (var ct in phieu.ChiTietPhieuMuons)
            {
                var sach = await _sachDAL.GetById(ct.MaSach);
                if (sach != null) sach.SoLuong += ct.SoLuong;
            }

            await _dal.Context.SaveChangesAsync();
            
            return (soNgayTre, tienPhat);
        }

        // 3. LẤY DANH SÁCH ĐANG MƯỢN (CHO PHẦN TRẢ SÁCH)
        public async Task<List<PhieuMuonHienThiDTO>> GetPhieuDangMuon(string maDocGia)
        {
            var listRaw = await _dal.Context.PhieuMuons
                .Include(pm => pm.DocGia)            // Kèm thông tin Độc giả (để lấy HoTen)
                .Include(pm => pm.ChiTietPhieuMuons) // Kèm chi tiết
                .ThenInclude(ct => ct.Sach)          // Kèm thông tin sách
                .Where(pm => pm.MaDocGia == maDocGia && pm.NgayTraThucTe == null)
                .ToListAsync();

            var result = new List<PhieuMuonHienThiDTO>();
            foreach (var pm in listRaw)
            {
                string trangThai = "Đang mượn";
                if (pm.NgayTraDuKien < DateTime.Now) trangThai = "QUÁ HẠN";

                // Nối tên sách
                var tenSachStr = string.Join(", ", pm.ChiTietPhieuMuons.Select(ct => ct.Sach.TenSach));

                // Tính tổng tiền (Xử lý null cho DonGia nếu có)
                decimal tongTien = pm.ChiTietPhieuMuons.Sum(ct => (ct.DonGia ?? 0) * ct.SoLuong);

                result.Add(new PhieuMuonHienThiDTO
                {
                    MaPhieu = pm.MaPhieu,
                    HoTen = pm.DocGia?.HoTen ?? "Không rõ", // Lấy tên độc giả
                    NgayMuon = pm.NgayMuon,
                    NgayTraDuKien = pm.NgayTraDuKien ?? DateTime.Now,
                    TenSach = tenSachStr,
                    TongTien = tongTien,
                    TrangThai = trangThai
                });
            }
            return result;
        }
    }
}