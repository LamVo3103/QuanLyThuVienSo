using Microsoft.AspNetCore.Mvc;
using QuanLyThuVienSo.API.BUS;
using QuanLyThuVienSo.API.Models;
using QuanLyThuVienSo.API.DTO; // Nhớ thêm dòng này để dùng DTO

namespace QuanLyThuVienSo.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly SachBUS _bus;

        public BooksController(SachBUS bus)
        {
            _bus = bus;
        }

        // 1. LẤY DANH SÁCH (Giữ nguyên)
        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] string? keyword)
        {
            return Ok(await _bus.LayDanhSachSach(keyword));
        }

        // 2. THÊM SÁCH (Đã sửa để nhận DTO)
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] SachDTO request)
        {
            try
            {
                // Chuyển đổi từ DTO (Gọn) -> Entity (Đầy đủ để lưu DB)
                var sachEntity = new Sach
                {
                    MaSach = request.MaSach,
                    TenSach = request.TenSach,
                    TheLoai = request.TheLoai,
                    NhaXuatBan = request.NhaXuatBan,
                    NgayXuatBan = request.NgayXuatBan,
                    MaTacGia = request.MaTacGia,
                    SoLuong = request.SoLuong,
                    GiaTien = request.GiaTien
                };

                string message = await _bus.ThemSachMoi(sachEntity);
                return Ok(new { message = message, data = request });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 3. SỬA SÁCH (Đã sửa để nhận DTO)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, [FromBody] SachDTO request)
        {
            try
            {
                // Chuyển đổi từ DTO -> Entity
                var sachEntity = new Sach
                {
                    MaSach = id, // Lấy ID từ URL
                    TenSach = request.TenSach,
                    TheLoai = request.TheLoai,
                    NhaXuatBan = request.NhaXuatBan,
                    NgayXuatBan = request.NgayXuatBan,
                    MaTacGia = request.MaTacGia,
                    SoLuong = request.SoLuong,
                    GiaTien = request.GiaTien
                };

                await _bus.CapNhatSach(id, sachEntity);
                return Ok(new { message = "Cập nhật sách thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 4. XÓA SÁCH (Giữ nguyên)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            try
            {
                await _bus.XoaSach(id);
                return Ok(new { message = "Xóa sách thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}