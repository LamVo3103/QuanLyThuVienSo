using Microsoft.EntityFrameworkCore;
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.DAL
{
    public class TaiKhoanDAL
    {
        private readonly QuanLyThuVienSoContext _context;
        public TaiKhoanDAL(QuanLyThuVienSoContext context) { _context = context; }

        public async Task<TaiKhoan?> GetByUsername(string username)
        {
            return await _context.TaiKhoans.FindAsync(username);
        }

        public async Task<TaiKhoan?> Login(string username, string password)
        {
            return await _context.TaiKhoans
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task Add(TaiKhoan tk)
        {
            _context.TaiKhoans.Add(tk);
            await _context.SaveChangesAsync();
        }
    }
}