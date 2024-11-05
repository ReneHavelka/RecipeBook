﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
	public interface IApplicationDbContext
	{
		public DbSet<DishType> DishTypes { get; set; }
		public DbSet<Recipe> Recipes { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}