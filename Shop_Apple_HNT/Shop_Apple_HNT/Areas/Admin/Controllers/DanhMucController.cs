
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop_Apple_HNT.Models;
using Shop_Apple_HNT.Repository;

namespace Shop_Apple_HNT.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class DanhMucController : Controller
	{


		private readonly DataContext _dataContext;
		public DanhMucController(DataContext context)
		{
			_dataContext = context;
			

		}
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.DanhMucs.OrderByDescending(p => p.ID).ToListAsync()); // lấy all danhmuc
		}
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhMucModel danhmuc)
        {
            
            //kiểm tra tình trạng model
            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                danhmuc.Slug = danhmuc.Ten.Replace(" ", "-");
                var slug = await _dataContext.DanhMucs.FirstOrDefaultAsync(p => p.Slug == danhmuc.Slug); //tìm danhmuc 
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danhmuc đã có trong database");
                    return View(danhmuc);
                }
                
                _dataContext.Add(danhmuc);
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


            return View(danhmuc);
        }
        public async Task<IActionResult> Edit(int Id)
        {

            DanhMucModel danhmuc = await _dataContext.DanhMucs.FindAsync(Id); // tìm dựa vào id
            return View(danhmuc); 
        }
        public async Task<IActionResult> Delete(int Id)
        {
            DanhMucModel danhmuc = await _dataContext.DanhMucs.FindAsync(Id);
           
            _dataContext.DanhMucs.Remove(danhmuc); //xóa data sản phẩm
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Danh mục đã bị xóa";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DanhMucModel danhmuc)
        {

            //kiểm tra tình trạng model
            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                danhmuc.Slug = danhmuc.Ten.Replace(" ", "-");
                var slug = await _dataContext.DanhMucs.FirstOrDefaultAsync(p => p.Slug == danhmuc.Slug); //tìm danhmuc 
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danhmuc đã có trong database");
                    return View(danhmuc);
                }

                _dataContext.Update(danhmuc);
                await _dataContext.SaveChangesAsync();
                TempData["error"] = "Cập nhật danh mục thành công";
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


            return View(danhmuc);
        }
    }
}
