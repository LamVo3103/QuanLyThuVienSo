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
            var query = _context.DocGias
                .Include(dg => dg.PhieuMuons)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.HoTen.Contains(keyword) || x.MaDocGia.Contains(keyword));
            }
            return await query.ToListAsync();
        }

        // 2. Thêm hàm mới: Lấy danh sách đang quá hạn
        public async Task<List<DocGia>> GetDocGiaQuaHan(string? keyword)
        {
            // 1. Lọc cơ bản: Những người có phiếu quá hạn
            var query = _context.DocGias
                .Include(dg => dg.PhieuMuons)
                .Where(dg => dg.PhieuMuons.Any(pm => pm.NgayTraThucTe != null && pm.NgayTraDuKien < DateTime.Now))
                .AsQueryable();

            // 2. Lọc nâng cao: Nếu có từ khóa thì tìm theo Tên hoặc Mã
            if (!string.IsNullOrEmpty(keyword))
            {
                // Lưu ý: Dùng ToLower() để tìm không phân biệt hoa thường nếu cần
                query = query.Where(x => x.HoTen.Contains(keyword) || x.MaDocGia.Contains(keyword));
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