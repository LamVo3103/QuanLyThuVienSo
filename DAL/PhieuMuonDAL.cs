using Microsoft.EntityFrameworkCore;
using QuanLyThuVienSo.API.Models;

namespace QuanLyThuVienSo.API.DAL
{
    public class PhieuMuonDAL
    {
        private readonly QuanLyThuVienSoContext _context;

        public PhieuMuonDAL(QuanLyThuVienSoContext context)
        {
            _context = context;
        }

        public QuanLyThuVienSoContext Context => _context;

        public async Task<List<PhieuMuon>> GetAll()
        {
            return await _context.PhieuMuons
                .Include(x => x.DocGia) // Đã sửa: MaDocGiaNavigation -> DocGia
                .Include(x => x.ChiTietPhieuMuons)
                .ThenInclude(x => x.Sach) // Đã sửa: MaSachNavigation -> Sach
                .OrderByDescending(x => x.NgayMuon)
                .ToListAsync();
        }

        public async Task<PhieuMuon?> GetById(int id)
        {
            return await _context.PhieuMuons
                .Include(x => x.DocGia) // Đã sửa
                .Include(x => x.ChiTietPhieuMuons)
                .ThenInclude(x => x.Sach) // Đã sửa
                .FirstOrDefaultAsync(x => x.MaPhieu == id);
        }

        public async Task Add(PhieuMuon pm)
        {
            _context.PhieuMuons.Add(pm);
            await _context.SaveChangesAsync();
        }

        public async Task Update(PhieuMuon pm)
        {
            _context.PhieuMuons.Update(pm);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var pm = await GetById(id);
            if (pm != null)
            {
                _context.PhieuMuons.Remove(pm);
                await _context.SaveChangesAsync();
            }
        }
    }
}