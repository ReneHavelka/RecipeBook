using Application.Common.Interfaces;
using Application.Recipes.Queries;
using Domain.Entities;

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

			string randomFileName = Path.GetRandomFileName();
			string randomFileNameWithoutExt = Path.GetFileNameWithoutExtension(randomFileName);
			string tempPath = Path.GetTempPath();
			string clompleteTempFileName;

			switch (Path.GetExtension(specificRecipe.FileName))
			{
				case ".pdf":
					clompleteTempFileName = Path.Combine(tempPath, randomFileNameWithoutExt + ".pdf");
					await File.WriteAllBytesAsync(clompleteTempFileName, specificRecipe.RecipeInstr);
					System.Diagnostics.Process.Start($"C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe", clompleteTempFileName);
					break;
				case ".docx":
					clompleteTempFileName = Path.Combine(tempPath, randomFileNameWithoutExt + ".doc");
					await File.WriteAllBytesAsync(clompleteTempFileName, specificRecipe.RecipeInstr);
					System.Diagnostics.Process.Start($"C:\\Program Files\\Microsoft Office\\Office15\\WINWORD.exe", clompleteTempFileName);
					break;
			}
		}
	}
}
