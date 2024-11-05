using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Recipes.Commands
{
	public class DeleteRecipe
	{
		IApplicationDbContext _dbContext;

		public DeleteRecipe(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DoDeleteRecipe(int id, CancellationToken cancellationToken = default)
		{

			Recipe entity = await _dbContext.Recipes.FirstAsync(x => x.Id == id);

			_dbContext.Recipes.Remove(entity);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
