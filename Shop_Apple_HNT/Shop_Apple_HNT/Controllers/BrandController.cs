using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_Apple_HNT.Models;
using Shop_Apple_HNT.Repository;

namespace Shop_Apple_HNT.Controllers
{
	public class BrandController : Controller
	{
		private readonly DataContext _dataContext;
		public BrandController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index(string Slug = "")
		{
			BrandModel brand = _dataContext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();
			if (brand == null)
			{
				return RedirectToAction("Index");
			}
			var sanphamByBrand = _dataContext.SanPhams.Where(p => p.BrandId == brand.Id);
			return View(await sanphamByBrand.OrderByDescending(p => p.Id).ToListAsync());
		}
	}
}
