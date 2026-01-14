using Microsoft.AspNetCore.Mvc;
using QuanLyThuVienSo.API.BUS; // Gọi BUS
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class TacGiaController : ControllerBase
    {
        private readonly TacGiaBUS _bus;

        public TacGiaController(TacGiaBUS bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? keyword)
        {
            return Ok(await _bus.LayDanhSach(keyword));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TacGia tacGia)
        {
            try
            {
                await _bus.ThemTacGia(tacGia);
                return Ok(new { message = "Thêm tác giả thành công", data = tacGia });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TacGia request)
        {
            try
            {
                await _bus.CapNhatTacGia(id, request);
                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _bus.XoaTacGia(id);
                return Ok(new { message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}