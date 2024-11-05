using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
	{
		public virtual DbSet<DishType> DishTypes { get; set; }
		public virtual DbSet<Recipe> Recipes { get; set; }
	}
}
