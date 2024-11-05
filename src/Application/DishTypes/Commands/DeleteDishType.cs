using Application.Common.Interfaces;
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

		public async Task DoDeleteDishType(int id, CancellationToken cancellationToken)
		{
			DishType entity = await _dbContext.DishTypes.FirstAsync(x => x.Id == id);

			_dbContext.DishTypes.Remove(entity);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
