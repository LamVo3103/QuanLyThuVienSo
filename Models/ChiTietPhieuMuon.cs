using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVienSo.API.Models
{
    [Table("ChiTietPhieuMuon")]
    public class ChiTietPhieuMuon
    {
        public int MaPhieu { get; set; }
        public string MaSach { get; set; } = null!;

        public int SoLuong { get; set; }
        public decimal? DonGia { get; set; }

        [ForeignKey("MaPhieu")]
        public virtual PhieuMuon? PhieuMuon { get; set; }

        [ForeignKey("MaSach")]
        public virtual Sach? Sach { get; set; } 
    }
}