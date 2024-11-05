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

		//Načítaj názvy druhy jedál, t.j. názvy a id.
		public async Task<IList<DishType>> GetDishTypeListAsync() => await _dbContext.DishTypes.ToListAsync();
	}
}
