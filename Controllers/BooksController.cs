using Microsoft.AspNetCore.Mvc;
using QuanLyThuVienSo.API.BUS;
using QuanLyThuVienSo.API.Models;

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

        // 1. LẤY DANH SÁCH
        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] string? keyword)
        {
            return Ok(await _bus.LayDanhSachSach(keyword));
        }

        // 2. THÊM SÁCH (Đã có logic cộng dồn)
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Sach sach)
        {
            try
            {
                // Hàm này giờ trả về chuỗi thông báo (Thêm mới hay Cộng dồn)
                string message = await _bus.ThemSachMoi(sach);
                return Ok(new { message = message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 3. SỬA SÁCH (MỚI THÊM)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, [FromBody] Sach sach)
        {
            try
            {
                await _bus.CapNhatSach(id, sach);
                return Ok(new { message = "Cập nhật sách thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 4. XÓA SÁCH (MỚI THÊM)
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