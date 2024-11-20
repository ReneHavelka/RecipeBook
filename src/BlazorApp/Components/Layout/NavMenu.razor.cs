using Application.Common.Interfaces;
using Application.DishTypes.Queries;
using BlazorApp.Components.Pages.Recipes;
using Domain.Entities;
using Infrastructure;

namespace BlazorApp.Components.Layout
{
	public partial class NavMenu
	{
		IApplicationDbContext _context;
		public IList<DishType> dishTypeList = new List<DishType>();

		public NavMenu(ApplicationDbContext context)
		{
			_context = context;
		}

		protected async override Task OnInitializedAsync()
		{
			var getDishTypes = new GetDishTypes(_context);
			dishTypeList = await getDishTypes.GetDishTypeListAsync();
		}
	}
}