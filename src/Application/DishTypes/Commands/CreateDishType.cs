using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.DishTypes.Commands
{
	public class CreateDishType
	{
		IApplicationDbContext _dbContext;

		public CreateDishType(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DoCreateDishType(string dishTypeName, CancellationToken cancellationToken)
		{
			DishType entity = new DishType();

			entity.Name = dishTypeName;

			await _dbContext.DishTypes.AddAsync(entity);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
