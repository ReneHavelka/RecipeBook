using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Recipes.Queries
{
	//Recepty - id a názvy
	public class GetRecipesIdsNames
	{
		IApplicationDbContext _dbContext;
		public GetRecipesIdsNames(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IList<Recipe>> GetRecipeListAsync(int dishTypeId = 0)
		{
			if (dishTypeId == 0)
			{
				return await _dbContext.Recipes.Select(x => new Recipe() { Id = x.Id, DishTypeId = x.DishTypeId, Name = x.Name })
											.OrderBy(x => x.DishTypeId).ThenBy(x => x.Name).ToListAsync();
			}

			return await _dbContext.Recipes.Where(x => x.DishTypeId == dishTypeId)
											.Select(x => new Recipe() { Id = x.Id, DishTypeId = x.DishTypeId, Name = x.Name }).OrderBy(x => x.Name).ToListAsync();
		}
	}
}
