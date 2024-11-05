using Application.Common.Interfaces;
using Application.Recipes.Queries;
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using RecipeBookTests.Application.Common;
using RecipeBookTests.Application.Recipes.Commands;
using System.Threading;

namespace RecipeBookTests.Application.Recipes.Queries
{
	[TestClass]
	public class GetRecipesIdsNamesTest
	{
		public static Semaphore semaphore = new Semaphore(0, 2);

		[TestMethod]
		public async Task TestGetRecipeListAsync()
		{
			//Zablokuj a čakaj na ukončenie dvoch testovacích metód - TestCreateRecipe a TestUpdateRecipe.
			semaphore.WaitOne();
			semaphore.WaitOne();

			ApplicationDbContext dbContext = new SetUpDbConext().CreateDbConext();
			//Arrange
			var allRecipesCount =  dbContext.Recipes.Count();

			//Act
			var getRecipesIdsNames = new GetRecipesIdsNames(dbContext);

			var recipeList = await getRecipesIdsNames.GetRecipeListAsync();

			//Pre otestovanie zoradenia podľa názvu jedla.
			var recipeListOrderedAsc = recipeList.OrderBy(x => x.Name).ToList();
			var recipeListOrderedDesc = recipeList.OrderByDescending(x => x.Name).ToList();

			//Assert
			Assert.AreEqual(allRecipesCount, recipeList.Count);

			//Testovanie zoradenia podľa názvu jedla ale de facto aj podľa typu jedla.
			Assert.AreSame(recipeList[0], recipeListOrderedAsc[0]);
			Assert.AreSame(recipeList[recipeListOrderedDesc.Count - 1], recipeListOrderedAsc[recipeListOrderedDesc.Count - 1]);
			Assert.AreSame(recipeList[0], recipeListOrderedDesc[recipeListOrderedDesc.Count - 1]);
			Assert.AreSame(recipeList[recipeListOrderedDesc.Count - 1], recipeListOrderedDesc[0]);

			
		}
	}
}
