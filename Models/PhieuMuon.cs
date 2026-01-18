using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVienSo.API.Models
{
    [Table("PhieuMuon")] // Hoặc tên bảng của bạn
    public class PhieuMuon
    {
        [Key]
        public int MaPhieu { get; set; }

        public string MaDocGia { get; set; } = null!;

        public DateTime NgayMuon { get; set; }
        public DateTime? NgayTraDuKien { get; set; }
        public DateTime? NgayTraThucTe { get; set; }
        public decimal TienPhat { get; set; } = 0;

        // 👇 BỔ SUNG DÒNG NÀY ĐỂ DÙNG ĐƯỢC .Include(pm => pm.DocGia)
        [ForeignKey("MaDocGia")]
        public virtual DocGia? DocGia { get; set; }

        // Danh sách chi tiết (Đã có sẵn)
        public virtual ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();
    }
}