using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Application.Recipes.Queries
{
	public class GetSpecificRecipe
	{
		IApplicationDbContext _dbContext;

		public GetSpecificRecipe(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Recipe> GetRecipeDirections(int id)
		{
			return await _dbContext.Recipes.Where(x => x.Id == id).Select(x => new Recipe() { RecipeInstr = x.RecipeInstr, FileName = x.FileName }).FirstOrDefaultAsync();
		}
	}
}
