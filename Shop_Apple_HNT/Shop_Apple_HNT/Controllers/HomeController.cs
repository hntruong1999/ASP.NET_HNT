using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_Apple_HNT.Models;
using Shop_Apple_HNT.Repository;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Shop_Apple_HNT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;
		public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _dataContext = context;
        }

        public IActionResult Index()
        {
            
            var sanphams = _dataContext.SanPhams.Include("DanhMuc").Include("Brand").ToList();
            return View(sanphams);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 404)
            {
                return View("404");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
		public async Task<IActionResult> Add(int Id)
		{
			SanPhamModel sanpham = await _dataContext.SanPhams.FindAsync(Id);
			List<GioHangItemModel> giohang = HttpContext.Session.GetJson<List<GioHangItemModel>>("GioHang") ?? new List<GioHangItemModel>();
			GioHangItemModel gioHangItems = giohang.Where(c => c.SanPhamId == Id).FirstOrDefault(); //kt sp có trong gio hang

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

        public IActionResult TimKiem(string keyword)
        {
            ViewBag.DanhMucs = _dataContext.DanhMucs.ToList();
            ViewBag.Brands = _dataContext.Brands.ToList();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var sanphams = _dataContext.SanPhams
                    .Include(s => s.DanhMuc)
                    .Include(s => s.Brand)
                    .Where(s =>
                        s.Ten.Contains(keyword) ||
                        s.DanhMuc.Ten.Contains(keyword) ||
                        s.Brand.Ten.Contains(keyword))
                    .ToList();

                return View("Index", sanphams);
            }

            return View("Index", new List<SanPhamModel>());
        }


        // Phương thức kiểm tra ký tự đặc biệt
        private bool IsAlphaNumeric(string text)
        {
            return !Regex.IsMatch(text, @"[^a-zA-Z0-9\s]");
        }




    }
}