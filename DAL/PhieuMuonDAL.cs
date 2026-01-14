using Microsoft.EntityFrameworkCore;
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.DAL
{
    public class PhieuMuonDAL
    {
        private readonly QuanLyThuVienSoContext _context;
        public PhieuMuonDAL(QuanLyThuVienSoContext context) { _context = context; }

        // Mượn DB Context để BUS quản lý Transaction (Cách đơn giản nhất cho đồ án)
        public QuanLyThuVienSoContext Context => _context; 

        public async Task<PhieuMuon?> GetById(int id)
        {
            return await _context.PhieuMuons
                .Include(p => p.ChiTietPhieuMuons)
                .FirstOrDefaultAsync(p => p.MaPhieu == id);
        }

        public async Task<List<PhieuMuon>> GetHistory(string maDocGia)
        {
            return await _context.PhieuMuons
                .Where(p => p.MaDocGia == maDocGia)
                .Include(p => p.ChiTietPhieuMuons).ThenInclude(ct => ct.MaSachNavigation)
                .OrderByDescending(p => p.NgayMuon).ToListAsync();
        }
    }
}