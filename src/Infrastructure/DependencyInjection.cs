using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
		{
			services.AddDbContextFactory<ApplicationDbContext>(options =>
			options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=RecipeBook;Trusted_Connection=True"), ServiceLifetime.Transient);

			//Published to Azure:
			//options.UseSqlServer(@"Server=tcp:rhserver.database.windows.net,1433;Initial Catalog=RecipeBook;Persist Security Info=False;User ID=***;Password=***;
			//	MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30"), ServiceLifetime.Transient);

			return services;
		}
	}
}
