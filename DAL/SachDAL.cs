using Microsoft.EntityFrameworkCore;
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.DAL
{
    public class SachDAL
    {
        private readonly QuanLyThuVienSoContext _context;

        public SachDAL(QuanLyThuVienSoContext context)
        {
            _context = context;
        }

        public async Task<List<Sach>> GetAll() => await _context.Saches.ToListAsync();
        
        public async Task<Sach?> GetById(string id) => await _context.Saches.FindAsync(id);
        
        // MỚI: Tìm sách theo tên chính xác (để check cộng dồn số lượng)
        public async Task<Sach?> GetByName(string tenSach)
        {
            return await _context.Saches.FirstOrDefaultAsync(s => s.TenSach.ToLower() == tenSach.ToLower());
        }

        public async Task<bool> Exists(string id) => await _context.Saches.AnyAsync(s => s.MaSach == id);

        // Kiểm tra xem sách này có đang bị mượn không (để chặn xóa)
        public async Task<bool> IsBorrowed(string id) => await _context.ChiTietPhieuMuons.AnyAsync(ct => ct.MaSach == id);

        public async Task Add(Sach sach)
        {
            _context.Saches.Add(sach);
            await _context.SaveChangesAsync();
        }

        // MỚI: Hàm cập nhật chung
        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        // MỚI: Hàm xóa
        public async Task Delete(Sach sach)
        {
            _context.Saches.Remove(sach);
            await _context.SaveChangesAsync();
        }
    }
}