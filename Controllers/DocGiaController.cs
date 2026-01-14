using Microsoft.AspNetCore.Mvc;
using QuanLyThuVienSo.API.BUS;
using QuanLyThuVienSo.API.Models;
using QuanLyThuVienSo.API.DTO; // Thêm dòng này

namespace QuanLyThuVienSo.API.Controllers
{
    [Route("api/readers")]
    [ApiController]
    public class DocGiaController : ControllerBase
    {
        private readonly DocGiaBUS _bus;

        public DocGiaController(DocGiaBUS bus)
        {
            _bus = bus;
        }

        // 1. LẤY DANH SÁCH (Giữ nguyên)
        [HttpGet]
        public async Task<IActionResult> GetDocGias([FromQuery] string? keyword)
        {
            var list = await _bus.GetAll(keyword);
            return Ok(list);
        }

        // 2. LẤY CHI TIẾT (Giữ nguyên)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocGia(string id)
        {
            try
            {
                var docGia = await _bus.GetById(id);
                return Ok(docGia);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // 3. THÊM ĐỘC GIẢ (Dùng DTO)
        [HttpPost]
        public async Task<IActionResult> CreateDocGia([FromBody] DocGiaDTO request)
        {
            try
            {
                // Mapping DTO -> Entity
                var docGiaEntity = new DocGia
                {
                    MaDocGia = request.MaDocGia,
                    HoTen = request.HoTen,
                    GioiTinh = request.GioiTinh,
                    NgaySinh = request.NgaySinh,
                    DiaChi = request.DiaChi,
                    DienThoai = request.DienThoai,
                    Cccd = request.Cccd,
                    NgayLamThe = DateTime.Now // Tự động lấy ngày hiện tại
                };

                await _bus.Add(docGiaEntity);
                return Ok(new { message = "Thêm độc giả thành công", data = request });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 4. SỬA THÔNG TIN (Dùng DTO)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocGia(string id, [FromBody] DocGiaDTO request)
        {
            try
            {
                // Mapping DTO -> Entity
                var docGiaEntity = new DocGia
                {
                    MaDocGia = id,
                    HoTen = request.HoTen,
                    GioiTinh = request.GioiTinh,
                    NgaySinh = request.NgaySinh,
                    DiaChi = request.DiaChi,
                    DienThoai = request.DienThoai,
                    Cccd = request.Cccd
                    // Không update NgayLamThe
                };

                await _bus.Update(id, docGiaEntity);
                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 5. XÓA ĐỘC GIẢ (Giữ nguyên)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocGia(string id)
        {
            try
            {
                await _bus.Delete(id);
                return Ok(new { message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}