using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop_Apple_HNT.Models;
using System.Security.Principal;

namespace Shop_Apple_HNT.Repository
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
		// ...
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<SanPhamModel>()
				.Property(p => p.Gia)
				.HasColumnType("decimal(10, 0)");

			modelBuilder.Entity<ChiTietDonHangModel>()
			   .Property(p => p.Gia)
			   .HasColumnType("decimal(10, 0)");

			modelBuilder.Entity<GioHangItemModel>()
			   .Property(p => p.Gia)
			   .HasColumnType("decimal(10, 0)");
		}

		public DbSet<BrandModel> Brands { get; set; }
        public DbSet<SanPhamModel> SanPhams { get; set; }
        public DbSet<DanhMucModel> DanhMucs { get; set; }
		public DbSet<ChiTietDonHangModel> ChiTietDonHang { get; set; }
		public DbSet<DatHangModel> DatHang { get; set; }
	}
}
