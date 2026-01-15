using QuanLyThuVienSo.API.DAL;
using QuanLyThuVienSo.API.Models;
using QuanLyThuVienSo.API.DTO;

namespace QuanLyThuVienSo.API.BUS
{
    public class DocGiaBUS
    {
        private readonly DocGiaDAL _dal;
        public DocGiaBUS(DocGiaDAL dal) { _dal = dal; }

        // --- HÀM MỚI: TỰ ĐỘNG TÍNH TRẠNG THÁI (Đã sửa lỗi Null) ---
        private DocGiaDTO MapToDTO(DocGia dg)
        {
            string trangThai = "Chưa mượn sách";

            // Lấy danh sách phiếu đang mượn (Chưa trả)
            var phieuDangMuon = dg.PhieuMuons?.Where(pm => pm.NgayTraThucTe == null).ToList();

            if (phieuDangMuon != null && phieuDangMuon.Count > 0)
            {
                // Nếu có bất kỳ phiếu nào quá hạn -> Gán luôn là QUÁ HẠN
                if (phieuDangMuon.Any(pm => pm.NgayTraDuKien < DateTime.Now))
                {
                    trangThai = "Quá hạn mượn sách";
                }
                else
                {
                    trangThai = "Đang mượn sách";
                }
            }

            return new DocGiaDTO
            {
                MaDocGia = dg.MaDocGia,
                HoTen = dg.HoTen,
                
                // 👇 SỬA LỖI Ở ĐÂY (Thêm giá trị mặc định nếu null)
                GioiTinh = dg.GioiTinh ?? "Khác", 
                NgaySinh = dg.NgaySinh ?? DateTime.Now, 
                
                DiaChi = dg.DiaChi,
                DienThoai = dg.DienThoai,
                Cccd = dg.Cccd,
                TrangThaiMuon = trangThai 
            };
        }

        // 1. LẤY TẤT CẢ (Trả về DTO)
        public async Task<List<DocGiaDTO>> GetAll(string? keyword)
        {
            var listEntity = await _dal.GetAll(keyword);
            return listEntity.Select(dg => MapToDTO(dg)).ToList();
        }

        // 2. LẤY DANH SÁCH QUÁ HẠN (Mới)
        public async Task<List<DocGiaDTO>> LayDanhSachQuaHan()
        {
            var listEntity = await _dal.GetDocGiaQuaHan();
            return listEntity.Select(dg => MapToDTO(dg)).ToList();
        }

        // 3. LẤY CHI TIẾT
        public async Task<DocGia> GetById(string id)
        {
            var dg = await _dal.GetById(id);
            if (dg == null) throw new Exception("Không tìm thấy độc giả");
            return dg;
        }

        // 4. THÊM
        public async Task Add(DocGia dg)
        {
            if (await _dal.Exists(dg.MaDocGia)) throw new Exception("Mã độc giả đã tồn tại");
            if (await _dal.ExistsCCCD(dg.Cccd ?? "")) throw new Exception("CCCD đã tồn tại");
            await _dal.Add(dg);
        }

        // 5. SỬA
        public async Task Update(string id, DocGia request)
        {
            var dg = await _dal.GetById(id);
            if (dg == null) throw new Exception("Không tìm thấy độc giả");
            
            dg.HoTen = request.HoTen;
            dg.GioiTinh = request.GioiTinh;
            dg.NgaySinh = request.NgaySinh;
            dg.DiaChi = request.DiaChi;
            dg.DienThoai = request.DienThoai;
            dg.Cccd = request.Cccd;
            // Không update NgayLamThe
            
            await _dal.Update();
        }

        // 6. XÓA
        public async Task Delete(string id)
        {
            var dg = await _dal.GetById(id);
            if (dg == null) throw new Exception("Không tìm thấy độc giả");
            if (await _dal.HasLoans(id)) throw new Exception("Độc giả này đang có lịch sử mượn sách, không thể xóa!");
            
            await _dal.Delete(dg);
        }
    }
}