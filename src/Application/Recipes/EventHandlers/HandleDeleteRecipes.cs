using Application.Common.Interfaces;
using Application.Recipes.Commands;

namespace Application.Recipes.EventHandlers
{
	public class HandleDeleteRecipes
	{
		IApplicationDbContext _dbContext;


		public HandleDeleteRecipes(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DoDeleteRecipes(IDictionary<int, bool> recipesToDelete)
		{
			var recipeIds = recipesToDelete.Where(x => x.Value == true).Select(x => x.Key);

			var deleteRecipe = new DeleteRecipe(_dbContext);
			await deleteRecipe.DoDeleteRecipe(recipeIds);
		}
	}
}
