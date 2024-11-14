using Application.Common.Interfaces;
using Application.Recipes.Commands;
using Application.Recipes.Validation;
using Domain.Entities;

namespace Application.Recipes.EventHandlers
{
	public class HandleCreateRecipe
	{
		IApplicationDbContext _dbContext;

		public HandleCreateRecipe(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<string> DoCreateRecipe(int dishTypeID, Stream stream, string name)
		{
			var submitValidation = new SubmitValidator();
			var validationResponse = submitValidation.ValidateSubmit(dishTypeID, stream, name);

			if (validationResponse.ToString() == "done")
			{
				byte[] recipeInstructions;
				using (var memoryStream = new MemoryStream())
				{
					await stream.CopyToAsync(memoryStream);
					recipeInstructions = memoryStream.ToArray();
				}

				Recipe recipe = new();
				recipe.DishTypeId = dishTypeID;
				recipe.Name = name;
				recipe.RecipeInstr = recipeInstructions;

				CreateRecipe createRecipe = new(_dbContext);
				await createRecipe.DoCreateRecipeAsync(recipe);
			}

			return validationResponse;
		}
	}
}
