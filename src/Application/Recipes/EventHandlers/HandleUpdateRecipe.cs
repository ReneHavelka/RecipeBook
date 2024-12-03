using Application.Common.Interfaces;
using Application.Recipes.Commands;
using Domain.Entities;

namespace Application.Recipes.EventHandlers
{
	public class HandleUpdateRecipe
	{
		IApplicationDbContext _dbContext;

		public HandleUpdateRecipe(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<string> DoUpdateRecipe(int recipeId, int dishTypeID, Stream stream, string name)
		{
			Recipe _recipe = await _dbContext.Recipes.FindAsync(recipeId);

			if (dishTypeID != 0) { _recipe.DishTypeId = dishTypeID; }

			if (stream != null)
			{
				byte[] recipeInstructions;
				using (var memoryStream = new MemoryStream())
				{
					await stream.CopyToAsync(memoryStream);
					recipeInstructions = memoryStream.ToArray();
				}
				_recipe.RecipeInstr = recipeInstructions;
			}

			if (name != null && name.Trim() != String.Empty) { _recipe.Name = name; }

			if (_recipe == null) { return String.Empty; }

			UpdateRecipe updateRecipe = new(_dbContext);
			await updateRecipe.DoUpdateRecipeAsync(_recipe);

			return "recipeUpdated";
		}

	}
}
