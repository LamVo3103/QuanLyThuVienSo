using QuanLyThuVienSo.API.DAL;
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.BUS
{
    public class SachBUS
    {
        private readonly SachDAL _dal;

        public SachBUS(SachDAL dal)
        {
            _dal = dal;
        }

        public async Task<List<Sach>> LayDanhSachSach(string? keyword)
        {
            var list = await _dal.GetAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                return list.Where(s => s.TenSach.Contains(keyword) || s.MaSach.Contains(keyword)).ToList();
            }
            return list;
        }

        public async Task<Sach?> LayChiTiet(string id) => await _dal.GetById(id);

        // --- LOGIC QUAN TRỌNG: THÊM SÁCH ---
        public async Task<string> ThemSachMoi(Sach sach)
        {
            // 1. Kiểm tra xem có sách nào trùng tên không?
            var sachTrungTen = await _dal.GetByName(sach.TenSach);
            
            if (sachTrungTen != null)
            {
                // Logic A: Nếu trùng tên -> Cộng dồn số lượng
                sachTrungTen.SoLuong += sach.SoLuong;
                await _dal.Update(); // Lưu thay đổi
                return $"Sách '{sach.TenSach}' đã có trong kho. Đã cộng thêm {sach.SoLuong} cuốn.";
            }
            else
            {
                // Logic B: Nếu chưa có tên -> Kiểm tra trùng Mã Sách
                if (await _dal.Exists(sach.MaSach))
                {
                    throw new Exception($"Mã sách '{sach.MaSach}' đã tồn tại! Vui lòng chọn mã khác.");
                }

                // Nếu mọi thứ ok -> Thêm mới
                await _dal.Add(sach);
                return "Thêm sách mới thành công!";
            }
        }

        // --- SỬA SÁCH ---
        public async Task CapNhatSach(string id, Sach request)
        {
            var sach = await _dal.GetById(id);
            if (sach == null) throw new Exception("Không tìm thấy sách cần sửa");

            sach.TenSach = request.TenSach;
            
            // --- SỬA LẠI DÒNG NÀY ---
            // Cũ (Sai): sach.TacGia = request.TacGia;
            // Mới (Đúng): Gán mã tác giả mới
            sach.MaTacGia = request.MaTacGia; 
            // ------------------------

            sach.TheLoai = request.TheLoai;
            sach.NhaXuatBan = request.NhaXuatBan;
            sach.GiaTien = request.GiaTien;
            
            // Không cho sửa Số lượng (vì số lượng liên quan đến kho và mượn trả)
            // Nếu muốn nhập thêm hàng, hãy dùng chức năng Thêm Sách (nó sẽ tự cộng dồn)

            await _dal.Update();
        }

        // --- XÓA SÁCH ---
        public async Task XoaSach(string id)
        {
            var sach = await _dal.GetById(id);
            if (sach == null) throw new Exception("Không tìm thấy sách");

            // Kiểm tra ràng buộc: Sách đang được mượn thì không được xóa
            if (await _dal.IsBorrowed(id))
            {
                throw new Exception("Sách này đang có người mượn hoặc nằm trong lịch sử phiếu mượn, không thể xóa!");
            }

            await _dal.Delete(sach);
        }
    }
}