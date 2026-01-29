using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_Apple_HNT.Repository;

namespace Shop_Apple_HNT.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly DataContext _dataContext;
        public SanPhamController(DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ChiTietSanPham(int Id)
        {
            if(Id==null)
            {
                return RedirectToAction("Index");
            }
            var sanphamById = _dataContext.SanPhams.Where(p => p.Id == Id).FirstOrDefault();
            return View(sanphamById);
        }
        public IActionResult LocSanPhamThapNhat()
        {
            var sanPhamThapNhat = _dataContext.SanPhams.OrderBy(p => p.Gia).ToList();
            return View(sanPhamThapNhat);
        }

        public IActionResult LocSanPhamCaoNhat()
        {
            var sanPhamCaoNhat = _dataContext.SanPhams.OrderByDescending(p => p.Gia).ToList();
            return View(sanPhamCaoNhat);
        }

    }
}
