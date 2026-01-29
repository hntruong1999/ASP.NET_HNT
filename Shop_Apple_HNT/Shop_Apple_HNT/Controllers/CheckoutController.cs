using Microsoft.AspNetCore.Mvc;
using Shop_Apple_HNT.Models;
using Shop_Apple_HNT.Models.ViewModels;
using Shop_Apple_HNT.Repository;
using System.Security.Claims;

namespace Shop_Apple_HNT.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		public CheckoutController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login", "TaiKhoan");
			}
			else
			{
				var maDatHang = Guid.NewGuid().ToString(); // tạo ra chuỗi từa lưa
				var datHangItem = new DatHangModel();
				datHangItem.MaDatHang = maDatHang;
				datHangItem.UserName = userEmail;
				datHangItem.Status = 1;// 1 nghĩa là đơn hàng mới
				datHangItem.NgayDat = DateTime.Now;
				_dataContext.Add(datHangItem);
				_dataContext.SaveChanges();
				TempData["success"] = "Tạo đơn hàng thành công";
				List<GioHangItemModel> giohangItems = HttpContext.Session.GetJson<List<GioHangItemModel>>("GioHang") ?? new List<GioHangItemModel>();
				foreach(var giohang in giohangItems) 
				{
					var chitietdonhang=new ChiTietDonHangModel();
					chitietdonhang.UserName=userEmail;
					chitietdonhang.MaDatHang = maDatHang;
                    chitietdonhang.SanPhamId = (int)giohang.SanPhamId;

                    chitietdonhang.Gia=giohang.Gia;
					chitietdonhang.SoLuong=giohang.SoLuong;
					_dataContext.Add(chitietdonhang);
					_dataContext.SaveChanges();
				}
				HttpContext.Session.Remove("GioHang");
				TempData["success"] = "Tạo thành công,chờ giao hàng";
				return RedirectToAction("Index", "GioHang");
			}
			return View();
		}
	}
}
