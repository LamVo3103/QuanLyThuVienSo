using QuanLyThuVienSo.API.DAL;
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.BUS
{
    public class TaiKhoanBUS
    {
        private readonly TaiKhoanDAL _dal;
        public TaiKhoanBUS(TaiKhoanDAL dal) { _dal = dal; }

        public async Task<TaiKhoan> Login(string username, string password)
        {
            var user = await _dal.Login(username, password);
            if (user == null) throw new Exception("Sai tài khoản hoặc mật khẩu!");
            return user;
        }

        public async Task Register(string username, string password)
        {
            if (await _dal.GetByUsername(username) != null)
                throw new Exception("Tên đăng nhập đã tồn tại!");

            var newUser = new TaiKhoan { Username = username, Password = password, Role = "User" };
            await _dal.Add(newUser);
        }
    }
}