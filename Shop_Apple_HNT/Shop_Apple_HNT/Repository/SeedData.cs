using Microsoft.EntityFrameworkCore;
using Shop_Apple_HNT.Models;

namespace Shop_Apple_HNT.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.SanPhams.Any())
            {
                DanhMucModel macbook = new DanhMucModel { Ten = "macbook", Slug = "macbook", MoTa = "macbook good", Status = 1 };
                DanhMucModel pc = new DanhMucModel { Ten = "pc", Slug = "pc", MoTa = "pc good", Status = 1 };

                BrandModel apple = new BrandModel { Ten = "Apple", Slug = "apple", Mota = "Apple good", Status = 1 };
                BrandModel samsung = new BrandModel { Ten = "samsung", Slug = "samsung", Mota = "samsung good", Status = 1 };
                _context.SanPhams.AddRange(
                    new SanPhamModel { Ten = "Macbook", Slug = "macbook", MoTa = "Macbook good", Hinh = "hinh.jpg", DanhMuc = macbook, Brand = apple, Gia = 1200 },
                    new SanPhamModel { Ten = "pc", Slug = "pc", MoTa = "pc good", Hinh = "hinh1.jpg", DanhMuc = pc, Brand = samsung, Gia = 1300 }
                );
                _context.SaveChanges();
            }
        }
    }
}
