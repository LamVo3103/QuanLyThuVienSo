using Microsoft.AspNetCore.Mvc;
using QuanLyThuVienSo.API.BUS;
using QuanLyThuVienSo.API.DTO;

namespace QuanLyThuVienSo.API.Controllers
{
    [Route("api/loans")]
    [ApiController]
    public class PhieuMuonController : ControllerBase
    {
        private readonly PhieuMuonBUS _bus;
        public PhieuMuonController(PhieuMuonBUS bus) { _bus = bus; }

        [HttpPost]
        public async Task<IActionResult> TaoPhieu([FromBody] TaoPhieuRequest request)
        {
            try {
                var id = await _bus.MuonSach(request.MaDocGia, request.DanhSachSachMuon);
                return Ok(new { message = "Mượn thành công", maPhieu = id });
            } catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut("return/{id}")]
        public async Task<IActionResult> TraSach(int id)
        {
            try {
                await _bus.TraSach(id);
                return Ok(new { message = "Trả sách thành công" });
            } catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}