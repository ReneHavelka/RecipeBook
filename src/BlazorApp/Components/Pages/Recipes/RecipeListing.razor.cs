using Application.Common.Interfaces;
using Application.Recipes.Commands;
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
		IDictionary<int, bool> RecipesToDelete { get; set; }

		[Inject]
		private protected NavigationManager Navigation { get; set; }


		public RecipeListing(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		protected async override Task OnParametersSetAsync()
		{
			var getRecepies = new GetRecipes(_dbContext);

			if (Id == null) return;

			RecipeList = await getRecepies.GetRecipeIdsDishTypesIdsNamesListAsync((int)Id);
			if (Id == -1) { DishTypeName = "V�etky recepty"; }

			RecipesToDelete = RecipeList.Select(x => x.Id).ToDictionary(x => x, y => false);
		}

		private async Task RecipeDirectionsAync(Recipe recipe)
		{
			var handleSpecificRecipeDirections = new HandleSpecificRecipeDirections(_dbContext);
			await handleSpecificRecipeDirections.RecipeDirections(recipe);
		}

		private async Task ToDelete()
		{
			var recipeIds = RecipesToDelete.Where(x => x.Value == true).Select(x => x.Key);

			var deleteRecipe = new DeleteRecipe(_dbContext);
			await deleteRecipe.DoDeleteRecipe(recipeIds);

			Navigation.NavigateTo($"/Recipes/recipeListing?Id={Id}&DishTypeName={DishTypeName}", forceLoad: true);
		}
	}
}