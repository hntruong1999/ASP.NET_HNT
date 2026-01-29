using System.ComponentModel.DataAnnotations;

namespace Shop_Apple_HNT.Models
{
    public class DanhMucModel
    {
        [Key] //khóa chính
        public int ID { get; set; }
        [Required(ErrorMessage ="Hãy nhập tên danh mục")] 
        public string Ten { get; set; }

        public string? Slug { get; set; }

        public int Status { get; set;  }
        [Required( ErrorMessage = "Hãy nhập tên danh mục")]
        public string MoTa { get; set; }
        
    }
}
