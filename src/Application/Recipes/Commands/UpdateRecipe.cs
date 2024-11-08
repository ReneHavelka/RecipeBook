﻿using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Recipes.Commands
{
	public class UpdateRecipe
	{
		IApplicationDbContext _dbContext;

		public UpdateRecipe(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DoUpdateRecipeAsync(Recipe recipe, CancellationToken cancellationToken)
		{
			//Doplniť kontrolu na DishType.
			//Doplniť kontrolu na FileExtension
			_dbContext.Recipes.Update(recipe);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
