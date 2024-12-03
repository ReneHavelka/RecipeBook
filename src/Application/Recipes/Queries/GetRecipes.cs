using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace Application.Recipes.Queries
{
	//Recepty - id a názvy
	public class GetRecipes
	{
		IApplicationDbContext _dbContext;
		public GetRecipes(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IList<Recipe>> GetRecipeIdsDishTypesIdsNamesListAsync(int dishTypeId)
		{
			//dishTypeId == -1: vyberie všetky
			if (dishTypeId == -1)
			{
				var recipesQry = from recipe in _dbContext.Recipes
								 join dishType in _dbContext.DishTypes
								 on recipe.DishTypeId equals dishType.Id
								 orderby dishType.Order, recipe.Name
								 select  new Recipe() { Id = recipe.Id, Name = recipe.Name };
				return await recipesQry.ToListAsync();
			}

			//dishTypeId 1= 0: vyberie len tie, ktoré spadajú do prislušnej kategórie jedál.
			return await _dbContext.Recipes.Where(x => x.DishTypeId == dishTypeId)
											.Select(x => new Recipe() { Id = x.Id, DishTypeId = x.DishTypeId, Name = x.Name }).OrderBy(x => x.Name).ToListAsync();
		}

		public async Task<IList<Recipe>> GetCompleteRecipes(IEnumerable<int> dishTypeIds = null)
		{
			if (dishTypeIds != null)
			{
				return await _dbContext.Recipes.Where(x => dishTypeIds.Contains(x.DishTypeId)).ToListAsync();
			}

			return await _dbContext.Recipes.ToListAsync();
		}
	}
}
