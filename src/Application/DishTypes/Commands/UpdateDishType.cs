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

		public async Task AddDishTypes(IList<DishType> newDishTypes)
		{
			newDishTypes = newDishTypes.Where(x => x.Name.Trim() != String.Empty).ToList();
			
			if (newDishTypes.Count == 0) { return; }
			
			//if (newDishTypes[newDishTypes.Count - 1].Name == String.Empty) { newDishTypes.RemoveAt(newDishTypes.Count - 1); }
			
			await _dbContext.DishTypes.AddRangeAsync(newDishTypes);
			await _dbContext.SaveChangesAsync();
		}

		public async Task DoUpdateDishTypesAsync(IEnumerable<DishType> dishTypes, CancellationToken cancellationToken = default)
		{
			_dbContext.DishTypes.UpdateRange(dishTypes);
			await _dbContext.SaveChangesAsync();
		}
	}
}
