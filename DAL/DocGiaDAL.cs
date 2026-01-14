using Microsoft.EntityFrameworkCore;
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.DAL
{
    public class DocGiaDAL
    {
        private readonly QuanLyThuVienSoContext _context;
        public DocGiaDAL(QuanLyThuVienSoContext context) { _context = context; }

        public async Task<List<DocGia>> GetAll(string? keyword)
        {
            var query = _context.DocGias.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(d => (d.HoTen != null && d.HoTen.Contains(keyword)) 
                                      || (d.Cccd != null && d.Cccd.Contains(keyword)));
            }
            return await query.ToListAsync();
        }

        public async Task<DocGia?> GetById(string id) => await _context.DocGias.FindAsync(id);
        
        public async Task<bool> Exists(string id) => await _context.DocGias.AnyAsync(d => d.MaDocGia == id);
        
        public async Task<bool> ExistsCCCD(string cccd) => await _context.DocGias.AnyAsync(d => d.Cccd == cccd);

        public async Task Add(DocGia dg) { _context.DocGias.Add(dg); await _context.SaveChangesAsync(); }
        
        public async Task Update() { await _context.SaveChangesAsync(); } // EF Core tự track update
        
        public async Task Delete(DocGia dg) { _context.DocGias.Remove(dg); await _context.SaveChangesAsync(); }

        public async Task<bool> HasLoans(string id) => await _context.PhieuMuons.AnyAsync(p => p.MaDocGia == id);
    }
}