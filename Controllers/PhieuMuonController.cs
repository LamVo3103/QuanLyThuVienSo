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

        // MƯỢN SÁCH
        [HttpPost]
        public async Task<IActionResult> TaoPhieu([FromBody] TaoPhieuMuonDTO request)
        {
            try {
                var id = await _bus.MuonSach(request.MaDocGia, request.DanhSachSachMuon);
                return Ok(new { message = "Mượn thành công", maPhieu = id });
            } catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        // TRẢ SÁCH
        [HttpPut("return/{id}")]
        public async Task<IActionResult> TraSach(int id)
        {
            try {
                var (soNgayTre, tienPhat) = await _bus.TraSach(id);
                return Ok(new { message = "Trả sách thành công", soNgayTre, tienPhat });
            } catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        // LẤY DANH SÁCH ĐANG MƯỢN (Để hiển thị lên GridView Trả Sách)
        [HttpGet("borrowed/{maDocGia}")]
        public async Task<IActionResult> GetBorrowedList(string maDocGia)
        {
            try
            {
                var list = await _bus.GetPhieuDangMuon(maDocGia);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}