using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_Apple_HNT.Models;
using Shop_Apple_HNT.Repository;

namespace Shop_Apple_HNT.Controllers
{
    public class DanhMucController : Controller
    {
        private readonly DataContext _dataContext;
        public DanhMucController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index(string Slug = "")
        {
            DanhMucModel danhmuc= _dataContext.DanhMucs.Where(c =>c.Slug ==Slug).FirstOrDefault();
            if(danhmuc == null)
            {
                return RedirectToAction("Index");
            }
            var sanphamByDanhMuc = _dataContext.SanPhams.Where(p =>p.DanhMucId == danhmuc.ID);
            return View(await sanphamByDanhMuc.OrderByDescending(p =>p.Id).ToListAsync());
        }
    }
}
