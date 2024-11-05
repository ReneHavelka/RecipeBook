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

		//Načítaj názvy a id pre všetky recepty.
		public async Task<IList<Recipe>> GetRecipeListAsync() => await _dbContext.Recipes.Select(x => new Recipe() { Id = x.Id, DishTypeId = x.DishTypeId, Name = x.Name })
											.OrderBy(x => x.DishTypeId).ThenBy(x => x.Name).ToListAsync();

		//Načítaj názvy a id receptov pre konrétny druh jedla.
		public async Task<IList<Recipe>> GetRecipeListOfSpecificDishType(int id) => await _dbContext.Recipes.Where(x => x.DishTypeId == id)
											.Select(x => new Recipe() { Id = x.Id, Name = x.Name }).OrderBy(x => x.Name).ToListAsync();
	}
}
