using QuanLyThuVienSo.API.DAL;
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.BUS
{
    public class TacGiaBUS
    {
        private readonly TacGiaDAL _dal;

        public TacGiaBUS(TacGiaDAL dal)
        {
            _dal = dal;
        }

        public async Task<List<TacGia>> LayDanhSach(string? keyword)
        {
            return await _dal.GetAll(keyword);
        }

        public async Task ThemTacGia(TacGia tacGia)
        {
            if (await _dal.Exists(tacGia.MaTacGia))
            {
                throw new Exception("Mã tác giả này đã tồn tại!");
            }
            await _dal.Add(tacGia);
        }

        public async Task CapNhatTacGia(string id, TacGia request)
        {
            var tg = await _dal.GetById(id);
            if (tg == null) throw new Exception("Không tìm thấy tác giả");

            // Cập nhật thông tin
            tg.HoTen = request.HoTen;
            tg.GioiTinh = request.GioiTinh;
            tg.QueQuan = request.QueQuan;

            await _dal.Update();
        }

        public async Task XoaTacGia(string id)
        {
            var tg = await _dal.GetById(id);
            if (tg == null) throw new Exception("Không tìm thấy tác giả");

            // Logic nghiệp vụ: Không được xóa nếu đang có sách
            if (await _dal.HasBooks(id))
            {
                throw new Exception("Không thể xóa tác giả này vì thư viện đang có sách của họ!");
            }

            await _dal.Delete(tg);
        }
    }
}