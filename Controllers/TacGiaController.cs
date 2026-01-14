using Microsoft.AspNetCore.Mvc;
using QuanLyThuVienSo.API.BUS;
using QuanLyThuVienSo.API.Models;
using QuanLyThuVienSo.API.DTO; // Thêm dòng này

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

        // THÊM TÁC GIẢ (Dùng DTO)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TacGiaDTO request)
        {
            try
            {
                // Mapping
                var tacGiaEntity = new TacGia
                {
                    MaTacGia = request.MaTacGia,
                    HoTen = request.HoTen,
                    GioiTinh = request.GioiTinh,
                    QueQuan = request.QueQuan
                };

                await _bus.ThemTacGia(tacGiaEntity);
                return Ok(new { message = "Thêm tác giả thành công", data = request });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // SỬA TÁC GIẢ (Dùng DTO)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TacGiaDTO request)
        {
            try
            {
                // Mapping
                var tacGiaEntity = new TacGia
                {
                    MaTacGia = id,
                    HoTen = request.HoTen,
                    GioiTinh = request.GioiTinh,
                    QueQuan = request.QueQuan
                };

                await _bus.CapNhatTacGia(id, tacGiaEntity);
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