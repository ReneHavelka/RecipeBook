using Application.Common.Interfaces;
using Application.Recipes.Queries;
using Domain.Entities;
using System.Diagnostics;

namespace Application.Recipes.EventHandlers
{
	public class HandleSpecificRecipeDirections
	{
		IApplicationDbContext _dbContext;
		public HandleSpecificRecipeDirections(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<byte[]> RecipeDirections(int recipeId)
		{
			var getSpecificRecipe = new GetSpecificRecipe(_dbContext);
			var specificRecipe = await getSpecificRecipe.GetRecipeDirections(recipeId);
			var recipeDirections = specificRecipe.RecipeInstr;

			return recipeDirections;
		}
	}
}
