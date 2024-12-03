using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.DishTypes.Queries
{
	//Druhy jedál
	public class GetDishTypes
	{
		readonly IApplicationDbContext _dbContext;

		public GetDishTypes(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		//Načítaj názvy druhov jedál, t.j. názvy a id.
		public async Task<IList<DishType>> GetDishTypeListAsync()
		{
			var dishTypes = _dbContext.DishTypes;
			var lastDishType = await dishTypes.MaxAsync(x => x.Order);
			var dishTypesList = await dishTypes.OrderBy(x => x.Id == 0 ? lastDishType + 1 : x.Order).ToListAsync();

			return dishTypesList;
		}
	}
}
