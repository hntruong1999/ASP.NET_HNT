using System.ComponentModel.DataAnnotations;

namespace Shop_Apple_HNT.Models
{
    public class GioHangItemModel
	{
        [Key]
        public long SanPhamId { get; set; }
        public string SanPhamName { get;set; }
        public int SoLuong { get; set; }

       
        public decimal Gia { get; set; }

        public string Hinh { get; set; }

       
        public decimal Tong
        {
            get { return SoLuong * Gia; }
        }
        public GioHangItemModel()
        {

        }
        public GioHangItemModel(SanPhamModel sanpham)
        {
            SanPhamId= sanpham.Id;
            SanPhamName = sanpham.Ten;
            Gia = sanpham.Gia;
            SoLuong = 1;
            Hinh = sanpham.Hinh;
        }
    }
}
