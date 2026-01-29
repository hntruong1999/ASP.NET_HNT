using System.ComponentModel.DataAnnotations;

namespace Shop_Apple_HNT.Models
{
    public class BrandModel
    {
        [Key]
        public int Id { get; set; }
        [Required (ErrorMessage = "Hãy nhập tên thương hiệu")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Hãy nhập mô tả")] 

        public string Mota { get; set; }

        public string? Slug { get; set; }

        public int Status { get;set; }
    }
}
