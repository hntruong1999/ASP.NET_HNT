using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop_Apple_HNT.Models;
using Shop_Apple_HNT.Repository;
using System.Net.Sockets;
using System.Reflection;

namespace Shop_Apple_HNT.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class SanPhamController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostenvironment;
        public SanPhamController(DataContext context, IWebHostEnvironment webHostenvironment)
        {
            _dataContext = context;
            _webHostenvironment = webHostenvironment;

        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.SanPhams.OrderByDescending(p => p.Id).Include(p => p.Brand).Include(p =>p.DanhMuc).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DanhMucs = new SelectList(_dataContext.DanhMucs, "ID", "Ten");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Ten");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPhamModel sanpham)
        {
            ViewBag.DanhMucs = new SelectList(_dataContext.DanhMucs, "ID", "Ten", sanpham.DanhMucId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Ten", sanpham.BrandId);

            //kiểm tra tình trạng model
            if(ModelState.IsValid)
            {
                //code thêm dữ liệu
                sanpham.Slug = sanpham.Ten.Replace(" ", "-");
                var slug = await _dataContext.SanPhams.FirstOrDefaultAsync(p => p.Slug == sanpham.Slug); //tìm sản phảm dựa vào slug
                if(slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(sanpham);
                }
                if(sanpham.TaiHinh != null)
                    {
                        string uploadDir = Path.Combine(_webHostenvironment.WebRootPath,"media/sanphams");
                        string hinhTen = Guid.NewGuid().ToString() + "_" + sanpham.TaiHinh.FileName;
                        string filePath = Path.Combine(uploadDir, hinhTen); //up dduongwf daanx vaof media sanpham 

                        FileStream fs = new FileStream(filePath,FileMode.Create); //copy hình ảnh vào filePath

                        await sanpham.TaiHinh.CopyToAsync(fs);
                        fs.Close();
                        sanpham.Hinh = hinhTen;

                      
                }
                
                _dataContext.Add(sanpham);
                await _dataContext.SaveChangesAsync();
                TempData["error"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model đang bị lỗi á";
                List<string>errors= new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach(var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }


            return View(sanpham);  
        }
        public async Task<IActionResult> Edit(int Id)
        {
            SanPhamModel sanpham = await _dataContext.SanPhams.FindAsync(Id);
            ViewBag.DanhMucs = new SelectList(_dataContext.DanhMucs, "ID", "Ten", sanpham.DanhMucId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Ten", sanpham.BrandId);
            return View(sanpham);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, SanPhamModel sanpham)
        {
            ViewBag.DanhMucs = new SelectList(_dataContext.DanhMucs, "ID", "Ten", sanpham.DanhMucId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Ten", sanpham.BrandId);

            //kiểm tra tình trạng model
            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                sanpham.Slug = sanpham.Ten.Replace(" ", "-");
                var slug = await _dataContext.SanPhams.FirstOrDefaultAsync(p => p.Slug == sanpham.Slug); //tìm sản phảm dựa vào slug
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sanr phaamr ddax cos trong database");
                    return View(sanpham);
                }
                if (sanpham.TaiHinh != null)
                {
                    string uploadDir = Path.Combine(_webHostenvironment.WebRootPath, "media/sanphams");
                    string hinhTen = Guid.NewGuid().ToString() + "_" + sanpham.TaiHinh.FileName;
                    string filePath = Path.Combine(uploadDir, hinhTen); //up dduongwf daanx vaof media sanpham 

                    FileStream fs = new FileStream(filePath, FileMode.Create); //copy hình ảnh vào filePath

                    await sanpham.TaiHinh.CopyToAsync(fs);
                    fs.Close();
                    sanpham.Hinh = hinhTen;


                }

                _dataContext.Update(sanpham);
                await _dataContext.SaveChangesAsync();
                TempData["error"] = "Chỉnh sản phẩm thành công";
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


            return View(sanpham);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            SanPhamModel sanpham = await _dataContext.SanPhams.FindAsync(Id);
            if(!string.Equals(sanpham.Hinh, "noname.jpg")) //chọn hình ảnh mà ko phải noname.jpg
            {
                string uploadDir = Path.Combine(_webHostenvironment.WebRootPath, "media/sanphams");
                //string hinhTen = Guid.NewGuid().ToString() + "_" + sanpham.TaiHinh.FileName;
                string fileOld = Path.Combine(uploadDir, sanpham.Hinh); 
                if(System.IO.File.Exists(fileOld)) //nếu tên file ảnh có tồn tại
                {
                    System.IO.File.Delete(fileOld); // xóa file cũ
                }
            }
            _dataContext.SanPhams.Remove(sanpham); //xóa data sản phẩm
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Sản phẩm đã bị xóa";
            return RedirectToAction("Index");
        }

    }
    
}

