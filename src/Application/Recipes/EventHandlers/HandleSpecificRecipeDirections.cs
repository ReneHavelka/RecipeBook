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

		public async Task RecipeDirections(Recipe recipe)
		{
			var getSpecificRecipe = new GetSpecificRecipe(_dbContext);
			var specificRecipe = await getSpecificRecipe.GetRecipeDirections(recipe.Id);
			var recipeDirections = specificRecipe.RecipeInstr;

			string randomFileName = Path.GetRandomFileName();
			string randomFileNameWithoutExt = Path.GetFileNameWithoutExtension(randomFileName);
			string tempPath = Path.GetTempPath();
			string completeTempFileName = string.Empty;

			switch (recipeDirections)
			{
				case byte[] a when a[0] == 0x25 && a[1] == 0x50 && a[2] == 0x44 && a[3] == 0x46:
					completeTempFileName = Path.Combine(tempPath, randomFileNameWithoutExt + ".pdf");
					break;
				case byte[] a when a[0] == 0xD0 && a[1] == 0xCF && a[2] == 0x11 && a[3] == 0xE0 && a[4] == 0xA1 && a[5] == 0xB1 && a[6] == 0x1A && a[7] == 0xE1:
					completeTempFileName = Path.Combine(tempPath, randomFileNameWithoutExt + ".doc");
					break;
				case byte[] a when a[0] == 0x50 && a[1] == 0x4B && a[2] == 0x03 && a[3] == 0x04:
					completeTempFileName = Path.Combine(tempPath, randomFileNameWithoutExt + ".docx");
					break;
				default:
					completeTempFileName = Path.Combine(tempPath, randomFileNameWithoutExt + ".txt");
					break;
			}

			if (completeTempFileName != string.Empty)
			{
				await File.WriteAllBytesAsync(completeTempFileName, recipeDirections);
				Process.Start(new ProcessStartInfo
				{
					FileName = completeTempFileName,
					UseShellExecute = true
				});
			}
		}
	}
}
