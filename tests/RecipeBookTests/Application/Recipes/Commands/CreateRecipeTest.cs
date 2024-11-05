using Application.Recipes.Commands;
using Domain.Entities;
using Infrastructure;
using RecipeBookTests.Application.Common;
using RecipeBookTests.Application.Recipes.Queries;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace RecipeBookTests.Application.Recipes.Commands
{
	[TestClass]
	public class CreateRecipeTest
	{
		[TestMethod]
		public async Task TestCreateRecipe()
		{
			//Arrange
			ApplicationDbContext dbContext = new SetUpDbConext().CreateDbConext();
			Recipe recipe = new();
			CancellationToken token = new CancellationToken();

			recipe.DishTypeId = 1;
			recipe.Name = "Recipe Name" + new Random().Next(100, 1000).ToString();
			recipe.FileName = "Recipe FileName";
			recipe.RecipeInstr = Encoding.Default.GetBytes("Recipe Directions");
			recipe.VideoFileLink = "Video File Link";

			//Act
			var createRecipe = new CreateRecipe(dbContext);
			await createRecipe.DoCreateRecipeAsync(recipe);

			var createdRecipe = dbContext.Recipes.FirstOrDefault(x => x.Name == recipe.Name);
			dbContext.Recipes.Remove(createdRecipe);
			await dbContext.SaveChangesAsync();

			GetRecipesIdsNamesTest.semaphore.Release();

			//Assert
			Assert.AreEqual(recipe.RecipeInstr, createdRecipe.RecipeInstr);
		}

	}
}
