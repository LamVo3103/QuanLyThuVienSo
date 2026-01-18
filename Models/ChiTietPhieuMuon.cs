using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVienSo.API.Models
{
    [Table("ChiTietPhieuMuon")]
    public class ChiTietPhieuMuon
    {
        [Key]
        public int Id { get; set; } // Hoặc khóa chính của bạn

        public int MaPhieu { get; set; }
        public string MaSach { get; set; } = null!; // Đây chỉ là Khóa ngoại (chuỗi)

        public int SoLuong { get; set; }
        public decimal? DonGia { get; set; }

        [ForeignKey("MaPhieu")]
        public virtual PhieuMuon? PhieuMuon { get; set; }
        
        [ForeignKey("MaSach")]
        public virtual Sach? Sach { get; set; } 
    }
}