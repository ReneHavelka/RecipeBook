﻿using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Recipes.Commands
{
	public class CreateRecipe
	{
		IApplicationDbContext _dbContext;

		public CreateRecipe(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DoCreateRecipeAsync(Recipe recipe, CancellationToken cancellationToken = default)
		{
			await _dbContext.Recipes.AddAsync(recipe);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
