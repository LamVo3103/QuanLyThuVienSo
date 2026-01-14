using Microsoft.AspNetCore.Mvc;
using QuanLyThuVienSo.API.BUS; // Gọi BUS
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.Controllers
{
    [Route("api/readers")]
    [ApiController]
    public class DocGiaController : ControllerBase
    {
        private readonly DocGiaBUS _bus; // Khai báo BUS

        // Constructor tiêm BUS vào
        public DocGiaController(DocGiaBUS bus)
        {
            _bus = bus;
        }

        // 1. LẤY DANH SÁCH
        [HttpGet]
        public async Task<IActionResult> GetDocGias([FromQuery] string? keyword)
        {
            // Gọi BUS để lấy danh sách (BUS đã lo vụ lọc Null rồi)
            var list = await _bus.GetAll(keyword);
            return Ok(list);
        }

        // 2. LẤY CHI TIẾT
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

        // 3. THÊM ĐỘC GIẢ
        [HttpPost]
        public async Task<IActionResult> CreateDocGia([FromBody] DocGia docGia)
        {
            try
            {
                await _bus.Add(docGia);
                return Ok(new { message = "Thêm độc giả thành công", data = docGia });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 4. SỬA THÔNG TIN
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocGia(string id, [FromBody] DocGia request)
        {
            try
            {
                await _bus.Update(id, request);
                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 5. XÓA ĐỘC GIẢ
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