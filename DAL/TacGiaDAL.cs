using Microsoft.EntityFrameworkCore;
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.DAL
{
    public class TacGiaDAL
    {
        private readonly QuanLyThuVienSoContext _context;

        public TacGiaDAL(QuanLyThuVienSoContext context)
        {
            _context = context;
        }

        public async Task<List<TacGia>> GetAll(string? keyword)
        {
            var query = _context.TacGias.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.HoTen.Contains(keyword));
            }
            return await query.ToListAsync();
        }

        public async Task<TacGia?> GetById(string id)
        {
            return await _context.TacGias.FindAsync(id);
        }

        public async Task<bool> Exists(string id)
        {
            return await _context.TacGias.AnyAsync(t => t.MaTacGia == id);
        }

        // Kiểm tra xem tác giả này có sách nào không (để chặn xóa)
        public async Task<bool> HasBooks(string authorId)
        {
            return await _context.Saches.AnyAsync(s => s.MaTacGia == authorId);
        }

        public async Task Add(TacGia tacGia)
        {
            _context.TacGias.Add(tacGia);
            await _context.SaveChangesAsync();
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(TacGia tacGia)
        {
            _context.TacGias.Remove(tacGia);
            await _context.SaveChangesAsync();
        }
    }
}