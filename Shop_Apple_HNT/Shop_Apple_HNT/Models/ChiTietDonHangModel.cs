using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop_Apple_HNT.Models
{
    public class ChiTietDonHangModel
    {
        [Key]
        public int ID { get; set; }

        public string UserName { get; set; }
        public string MaDatHang { get; set; }

        [Required]
        [ForeignKey(nameof(SanPham))]
        public int SanPhamId { get; set; }

        public decimal Gia { get; set; }
        public int SoLuong { get; set; }

        public SanPhamModel SanPham { get; set; }
    }

}
