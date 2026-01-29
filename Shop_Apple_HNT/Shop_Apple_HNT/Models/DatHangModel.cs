using System.ComponentModel.DataAnnotations;

namespace Shop_Apple_HNT.Models
{
	public class DatHangModel
	{
		[Key]
		public int Id { get; set; }
		public string MaDatHang { get; set; }
		public string UserName { get; set;}
		public DateTime NgayDat { get; set;}
		public int Status { get; set; }
        // Danh sách các sản phẩm trong đơn hàng
        public ICollection<SanPhamModel>? SanPhams { get; set; }

    }
}
