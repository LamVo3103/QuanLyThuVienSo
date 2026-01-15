using Microsoft.AspNetCore.Mvc;
using QuanLyThuVienSo.API.BUS;
using QuanLyThuVienSo.API.Models;
using QuanLyThuVienSo.API.DTO;

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

        // 1. LẤY DANH SÁCH (Đã tự động tính trạng thái)
        [HttpGet]
        public async Task<IActionResult> GetDocGias([FromQuery] string? keyword)
        {
            // Hàm này giờ trả về List<DocGiaDTO> có trường TrangThaiMuon
            var list = await _bus.GetAll(keyword);
            return Ok(list);
        }

        // --- ENDPOINT MỚI: LẤY DANH SÁCH QUÁ HẠN ---
        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueReaders()
        {
            try
            {
                var list = await _bus.LayDanhSachQuaHan();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
        public async Task<IActionResult> CreateDocGia([FromBody] DocGiaDTO request)
        {
            try
            {
                var docGiaEntity = new DocGia
                {
                    MaDocGia = request.MaDocGia,
                    HoTen = request.HoTen,
                    GioiTinh = request.GioiTinh,
                    NgaySinh = request.NgaySinh,
                    DiaChi = request.DiaChi,
                    DienThoai = request.DienThoai,
                    Cccd = request.Cccd,
                    NgayLamThe = DateTime.Now
                };

                await _bus.Add(docGiaEntity);
                return Ok(new { message = "Thêm độc giả thành công", data = request });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 4. SỬA THÔNG TIN
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocGia(string id, [FromBody] DocGiaDTO request)
        {
            try
            {
                var docGiaEntity = new DocGia
                {
                    MaDocGia = id,
                    HoTen = request.HoTen,
                    GioiTinh = request.GioiTinh,
                    NgaySinh = request.NgaySinh,
                    DiaChi = request.DiaChi,
                    DienThoai = request.DienThoai,
                    Cccd = request.Cccd
                };

                await _bus.Update(id, docGiaEntity);
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