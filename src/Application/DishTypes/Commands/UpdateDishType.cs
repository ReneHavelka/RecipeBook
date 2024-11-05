using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.DishTypes.Commands
{
	public class UpdateDishType
	{
		IApplicationDbContext _dbContext;
		public UpdateDishType(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DoUpdateDishType(DishType dishType, CancellationToken cancellationToken)
		{
			DishType entity = new DishType();

			entity.Id = dishType.Id;
			entity.Name = dishType.Name;

			_dbContext.DishTypes.Update(entity);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
