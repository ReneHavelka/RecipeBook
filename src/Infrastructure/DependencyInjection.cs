using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
		{
			services.AddDbContextFactory<ApplicationDbContext>(options =>
					options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=RecipeBook;Trusted_Connection=True"), ServiceLifetime.Transient);

			return services;
		}
	}
}
