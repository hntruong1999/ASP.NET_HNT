using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shop_Apple_HNT.Repository.Components
{
	public class CategoriesViewComponent : ViewComponent
	{
		private readonly DataContext _dataContext;

		public CategoriesViewComponent(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.DanhMucs.ToListAsync());
	}
}
