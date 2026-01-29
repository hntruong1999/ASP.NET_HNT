using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Shop_Apple_HNT.Models;
using Shop_Apple_HNT.Models.ViewModels;
using Shop_Apple_HNT.Repository;

namespace Shop_Apple_HNT.Controllers
{
    public class GioHangController : Controller
    {
        private readonly DataContext _dataContext;

        public GioHangController(DataContext _context)
        {
            _dataContext = _context;
        }

        public IActionResult Index()
        {
            //List<GioHangItemModel> giohangItem = HttpContext.Session.GetJson<List<GioHangItemModel>>("GioHang") ?? new List<GioHangItemModel>();

            // Kiểm tra xem có danh sách sản phẩm trong giỏ hàng trong Session không
            List<GioHangItemModel> giohangItems = HttpContext.Session.GetJson<List<GioHangItemModel>>("GioHang") ?? new List<GioHangItemModel>();


            GioHangItemViewModel giohangVM = new()
            {
                GioHangItems = giohangItems,
                TongTien = giohangItems.Sum(x => x.SoLuong * x.Gia)
            };
            return View(giohangVM);
        }
        public ActionResult CheckOut()
        {
            return View("~/Views/CheckOut/Index.cshtml");

        }
        public async Task<IActionResult> Add(int Id)
        {
            SanPhamModel sanpham = await _dataContext.SanPhams.FindAsync(Id);
            List<GioHangItemModel> giohang = HttpContext.Session.GetJson<List<GioHangItemModel>>("GioHang") ?? new List<GioHangItemModel>();
            GioHangItemModel gioHangItems = giohang.Where(c =>c.SanPhamId==Id).FirstOrDefault(); //kt sp có trong gio hang

            if (gioHangItems == null)
            {
                giohang.Add(new GioHangItemModel(sanpham)); //add sp tìm đc dựa vào id
            }
            else
            {
                gioHangItems.SoLuong += 1;
            }
            HttpContext.Session.SetJson("GioHang", giohang); // save data vaò session sanpham
            return Redirect(Request.Headers["Referer"].ToString()); //return veef trang truocws ddos
        }
		public async Task<IActionResult> Giam(int Id) // hamf giảm số lượng
        {
            List<GioHangItemModel> giohang = HttpContext.Session.GetJson<List<GioHangItemModel>>("GioHang");
            GioHangItemModel gioHangItem = giohang.Where(c =>c.SanPhamId==Id).FirstOrDefault();
			if (gioHangItem != null) // Kiểm tra xem mục có tồn tại không
			{
				if (gioHangItem.SoLuong > 1)
				{
					gioHangItem.SoLuong--;
				}
				else
				{
					giohang.RemoveAll(p => p.SanPhamId == Id);
				}

				if (giohang.Count == 0)
				{
					HttpContext.Session.Remove("GioHang");
				}
				else
				{
					HttpContext.Session.SetJson("GioHang", giohang);
				}
			}

			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Tang(int Id) // hamf tăng số lượng( nút btn)
		{
			List<GioHangItemModel> giohang = HttpContext.Session.GetJson<List<GioHangItemModel>>("GioHang");
			GioHangItemModel gioHangItem = giohang.Where(c => c.SanPhamId == Id).FirstOrDefault();
			if (gioHangItem != null) // Kiểm tra xem mục có tồn tại không
			{
				if (gioHangItem.SoLuong >= 1)
				{
					gioHangItem.SoLuong++;
				}
				else
				{
					giohang.RemoveAll(p => p.SanPhamId == Id);
				}

				if (giohang.Count == 0)
				{
					HttpContext.Session.Remove("GioHang");
				}
				else
				{
					HttpContext.Session.SetJson("GioHang", giohang);
				}
			}
			TempData["success"] = "Thêm vào giỏ hàng thành công";
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Xoa(int Id)
		{
			List<GioHangItemModel> giohang = HttpContext.Session.GetJson<List<GioHangItemModel>>("GioHang");
			giohang.RemoveAll(p=>p.SanPhamId==Id);

			if (giohang.Count == 0)
			{
				HttpContext.Session.Remove("GioHang");
			
			}
			else
			{
				HttpContext.Session.SetJson("GioHang", giohang);
			}
			TempData["success"] = "Xóa sản phẩm trong giỏ hàng thành công";
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Clear()
		{
			HttpContext.Session.Remove("GioHang");
			TempData["success"] = "Xóa toàn bộ giỏ hàng thành công";
			return RedirectToAction("Index");
		}

	}

}
