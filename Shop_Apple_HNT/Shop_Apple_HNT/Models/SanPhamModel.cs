using Shop_Apple_HNT.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop_Apple_HNT.Models
{
    public class SanPhamModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Hãy nhập tên sản phẩm")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Hãy nhập mô tả sản phẩm")]
        public string MoTa { get; set; }

        [Required]
        public decimal Gia { get; set; }

        public string? Slug { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int DanhMucId { get; set; }

        public BrandModel? Brand { get; set; }
        public DanhMucModel? DanhMuc { get; set; }

        // Upload
        public string? Hinh { get; set; }
        [NotMapped]
        public IFormFile? TaiHinh { get; set; }

        // ✔ QUAN HỆ ĐÚNG
        public ICollection<ChiTietDonHangModel>? ChiTietDonHangs { get; set; }
    }
}
