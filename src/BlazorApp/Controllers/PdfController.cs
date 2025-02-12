using Application.Common.Interfaces;
using Application.Recipes.EventHandlers;
using Application.Recipes.Queries;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BlazorApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PdfController : ControllerBase
	{
		IApplicationDbContext _dbContext;
		public PdfController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet("{RecipeId}")]
		public async Task<IActionResult> GetPdf(int RecipeId)
		{
			var handleSpecificRecipeDirections = new HandleSpecificRecipeDirections(_dbContext);
			var recipeDirections = await handleSpecificRecipeDirections.RecipeDirections(RecipeId);

			return File(recipeDirections, "application/pdf");
		}
	}
}
