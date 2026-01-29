using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_Apple_HNT.Models;
using Shop_Apple_HNT.Repository;

namespace Shop_Apple_HNT.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext context)
        {
            _dataContext = context;


        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _dataContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
            }
            catch (Exception ex)
            {
                return Content(ex.Message); // Hoặc log ex.StackTrace để kiểm tra lỗi
            }
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int Id)
        {

            BrandModel brand = await _dataContext.Brands.FindAsync(Id); // tìm dựa vào id
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {

            //kiểm tra tình trạng model
            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                brand.Slug = brand.Ten.Replace(" ", "-");
                var slug = await _dataContext.DanhMucs.FirstOrDefaultAsync(p => p.Slug == brand.Slug); //tìm danhmuc 
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(brand);
                }

                _dataContext.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["error"] = "Thêm danh mục thành công";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model đang bị lỗi á";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }


            return View(brand);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);

            _dataContext.Brands.Remove(brand); //xóa data sản phẩm
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Danh mục đã bị xóa";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {

            //kiểm tra tình trạng model
            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                brand.Slug = brand.Ten.Replace(" ", "-");
                var slug = await _dataContext.DanhMucs.FirstOrDefaultAsync(p => p.Slug == brand.Slug); //tìm danhmuc 
                if (slug != null)
                {
                    ModelState.AddModelError("", "Thương hiệu đã có trong database");
                    return View(brand);
                }

                _dataContext.Update(brand);
                await _dataContext.SaveChangesAsync();
                TempData["error"] = "Cập nhật thương hiệu thành công";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model đang bị lỗi á";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }


            return View(brand);
        }
    }
}
