using Application.Recipes.Commands;
using Domain.Entities;
using Infrastructure;
using RecipeBookTests.Application.Common;
using RecipeBookTests.Application.Recipes.Queries;
using System.Text;

namespace RecipeBookTests.Application.Recipes.Commands
{
	[TestClass]
	public class UpdateRecipeTest
	{
		[TestMethod]
		public async Task TestUpdateRecipe()
		{
			//Arrange
			ApplicationDbContext dbContext = new SetUpDbConext().CreateDbConext();

			Recipe[] recipeArr =
				[   new Recipe
						{
							DishTypeId = 1,
							Name = string.Concat("Recipe Name", new Random().Next(100,1000)),
							FileName = "Recipe FileName1",
							RecipeInstr = Encoding.Default.GetBytes("Recipe Directions1"),
							VideoFileLink = "Video File Link1"
						},
					new Recipe
						{
							DishTypeId = 2,
							Name = string.Concat("Recipe Name", new Random().Next(100,1000)),
							FileName = "Recipe FileName2",
							RecipeInstr = Encoding.Default.GetBytes("Recipe Directions2"),
							VideoFileLink = "Video File Link2"
						}
				];

			await dbContext.Recipes.AddRangeAsync(recipeArr);
			await dbContext.SaveChangesAsync();

			//Act
			Recipe recipeToUpdate = dbContext.Recipes.Where(x => x.Name == recipeArr[0].Name).FirstOrDefault();
			recipeToUpdate.DishTypeId = 3;
			recipeToUpdate.Name = "Recipe Name Corrected" + new Random().Next(100, 1000);
			recipeToUpdate.FileName = "Recipe FileName Corrected";
			recipeToUpdate.RecipeInstr = Encoding.Default.GetBytes("Recipe Directions Corrected");
			await new UpdateRecipe(dbContext).DoUpdateRecipeAsync(recipeToUpdate, CancellationToken.None);

			Recipe updatedRecipe = dbContext.Recipes.Where(x => x.Id == recipeToUpdate.Id).FirstOrDefault();
			Recipe secondAddedRecipe = dbContext.Recipes.Where(x => x.Name == recipeArr[1].Name).FirstOrDefault();

			//Vymaž horeuvedené pridané záznamy.
			Array.Clear(recipeArr);
			recipeArr[0] = updatedRecipe;
			recipeArr[1] = secondAddedRecipe;

			dbContext.Recipes.RemoveRange(recipeArr);
			await dbContext.SaveChangesAsync();

			GetRecipesIdsNamesTest.semaphore.Release();

			//Assert
			Assert.AreEqual(recipeToUpdate.DishTypeId, updatedRecipe.DishTypeId);
			Assert.AreEqual(recipeToUpdate.Name, updatedRecipe.Name);
			Assert.AreEqual(recipeToUpdate.FileName, updatedRecipe.FileName);
			Assert.AreEqual(recipeToUpdate.RecipeInstr, updatedRecipe.RecipeInstr);
			Assert.AreEqual(recipeToUpdate.FileName, updatedRecipe.FileName);
		}
	}
}
