using Application.Common.Interfaces;

namespace Application.Recipes.Commands
{
	public class DeleteRecipe
	{
		IApplicationDbContext _dbContext;

		public DeleteRecipe(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DoDeleteRecipe(IEnumerable<int> ids, CancellationToken cancellationToken = default)
		{
			var entities = _dbContext.Recipes.Where(x => ids.Contains(x.Id));

			_dbContext.Recipes.RemoveRange(entities);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
