using Application.Common.Interfaces;
using Application.Recipes.Queries;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.DishTypes.Commands
{
	public class DeleteDishType
	{
		IApplicationDbContext _dbContext;

		public DeleteDishType(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DoDeleteDishType(IEnumerable<int> dishTypeIds, CancellationToken cancellationToken = default)
		{
			var entitiesToDelete = _dbContext.DishTypes.Where(x => dishTypeIds.Contains(x.Id));

			var getRecipes = new GetRecipes(_dbContext);
			var recipesSelected = await getRecipes.GetCompleteRecipes(dishTypeIds);

			foreach (var recipe in recipesSelected)
			{

				recipe.DishTypeId = 0;
			}

			_dbContext.Recipes.UpdateRange(recipesSelected);
			await _dbContext.SaveChangesAsync(cancellationToken);

			_dbContext.DishTypes.RemoveRange(entitiesToDelete);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
