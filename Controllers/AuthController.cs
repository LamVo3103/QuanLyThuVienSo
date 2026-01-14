using Microsoft.AspNetCore.Mvc;
using QuanLyThuVienSo.API.BUS;
using QuanLyThuVienSo.API.DTO; 

namespace QuanLyThuVienSo.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TaiKhoanBUS _bus;
        public AuthController(TaiKhoanBUS bus) { _bus = bus; }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try {
                var user = await _bus.Login(request.Username, request.Password);
                return Ok(new { message = "Đăng nhập thành công", role = user.Role, username = user.Username });
            } catch (Exception ex) { return Unauthorized(new { message = ex.Message }); }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try {
                await _bus.Register(request.Username, request.Password);
                return Ok(new { message = "Đăng ký thành công!" });
            } catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}