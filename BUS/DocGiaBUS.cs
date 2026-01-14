using QuanLyThuVienSo.API.DAL;
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.BUS
{
    public class DocGiaBUS
    {
        private readonly DocGiaDAL _dal;
        public DocGiaBUS(DocGiaDAL dal) { _dal = dal; }

        public async Task<List<DocGia>> GetAll(string? keyword) => await _dal.GetAll(keyword);

        public async Task<DocGia> GetById(string id)
        {
            var dg = await _dal.GetById(id);
            if (dg == null) throw new Exception("Không tìm thấy độc giả");
            return dg;
        }

        public async Task Add(DocGia dg)
        {
            if (await _dal.Exists(dg.MaDocGia)) throw new Exception("Mã độc giả đã tồn tại");
            
            if (await _dal.ExistsCCCD(dg.Cccd ?? "")) throw new Exception("CCCD đã tồn tại");
            
            await _dal.Add(dg);
        }

        public async Task Update(string id, DocGia request)
        {
            var dg = await _dal.GetById(id);
            if (dg == null) throw new Exception("Không tìm thấy độc giả");
            
            // Cập nhật thông tin
            dg.HoTen = request.HoTen;
            dg.GioiTinh = request.GioiTinh;
            dg.NgaySinh = request.NgaySinh;
            dg.DiaChi = request.DiaChi;
            dg.DienThoai = request.DienThoai;
            dg.Cccd = request.Cccd;
            dg.NgayLamThe = request.NgayLamThe;
            
            await _dal.Update();
        }

        public async Task Delete(string id)
        {
            var dg = await _dal.GetById(id);
            if (dg == null) throw new Exception("Không tìm thấy độc giả");
            if (await _dal.HasLoans(id)) throw new Exception("Độc giả này đang có lịch sử mượn sách, không thể xóa!");
            
            await _dal.Delete(dg);
        }
    }
}