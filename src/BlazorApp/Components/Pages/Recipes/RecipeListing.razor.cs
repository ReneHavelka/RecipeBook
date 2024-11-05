using Application.Common.Interfaces;
using Application.Recipes.EventHandlers;
using Application.Recipes.Queries;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata;

namespace BlazorApp.Components.Pages.Recipes
{
	public partial class RecipeListing
	{
		[Parameter]
		[SupplyParameterFromQuery]
		public int? Id { get; set; }
		[Parameter]
		[SupplyParameterFromQuery]
		public string DishTypeName { get; set; }

		IApplicationDbContext _dbContext;
		IList<Recipe> RecipeList { get; set; } = new List<Recipe>();

		[Inject]
		NavigationManager NvM { get; set; }


		public RecipeListing(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		protected async override Task OnParametersSetAsync()
		{
			var getRecepies = new GetRecipesIdsNames(_dbContext);

			if (Id == null) return;

			if (Id == 0)
			{
				RecipeList = await getRecepies.GetRecipeListAsync();
				DishTypeName = "Všetky recepty";
			}
			else
			{
				RecipeList = await getRecepies.GetRecipeListOfSpecificDishType((int)Id);
			}
		}

		private async Task RecipeDirections(Recipe recipe)
		{
			var handleSpecificRecipeDirections = new HandleSpecificRecipeDirections(_dbContext);
			await handleSpecificRecipeDirections.RecipeDirections(recipe);
		}
	}
}