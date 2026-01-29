

using System.ComponentModel.DataAnnotations;

namespace Shop_Apple_HNT.Models.ViewModels
{
    public class GioHangItemViewModel
    {
        public List<GioHangItemModel> GioHangItems { get;set; }
        
        public decimal Gia { get; set; }
        
        public decimal TongTien { get; internal set; }
	}

}
