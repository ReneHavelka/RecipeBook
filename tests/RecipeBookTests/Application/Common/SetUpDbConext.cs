using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace RecipeBookTests.Application.Common
{
	internal class SetUpDbConext
	{
		internal ApplicationDbContext CreateDbConext()
		{
			//Nastavenie dbContext - pre produkčnú DB, ale môže byť zvolená i testovacia DB. Mickrosoft mocking nedoporučuje. (Komplikované, správanie môže byť odlišné,
			//nemusí byť úplne spoľahlivé). Najvhodnejšie riešenie je preniesť produkčnú DB na vlastný počítač.
			DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			dbContextOptionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=RecipeBook;Trusted_Connection=True");
			DbContextOptions dbContextOptions = dbContextOptionsBuilder.Options;
			//DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer("Data Source=(localdb)\\test;Integrated Security=true;").Options;
			return new ApplicationDbContext((DbContextOptions<ApplicationDbContext>)dbContextOptions);
		}
	}
}
