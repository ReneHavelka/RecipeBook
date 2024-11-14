using Application.Common.Interfaces;
using Application.DishTypes.Queries;
using Application.Recipes.EventHandlers;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel;

namespace BlazorApp.Components.Pages.Recipes
{
	public partial class CreateRecipe
	{
		IApplicationDbContext _dbContext;

		[SupplyParameterFromForm]
		private Recipe RecipeModel { get; set; }

		private IList<DishType> dishTypeList = new List<DishType>();
		private IBrowserFile file;
		private string FileName;

		private string DishTypeIdWarning { get; set; }
		private string FileWarning { get; set; }
		private string NameWarning { get; set; }
		private string DoneNotification { get; set; } = string.Empty;


		public CreateRecipe(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		protected override async Task OnInitializedAsync()
		{
			RecipeModel = new();

			GetDishTypes getDishTypes = new(_dbContext);
			dishTypeList = await getDishTypes.GetDishTypeListAsync();
		}

		private void DishTypeSelected()
		{
			if (RecipeModel.DishTypeId > 0) { DishTypeIdWarning = string.Empty; }
		}

		private void LoadFile(InputFileChangeEventArgs e)
		{
			file = e.File;
			FileWarning = string.Empty;
			FileName = file.Name;
		}

		public void DishNameEntered()
		{
			NameWarning = string.Empty;
		}

		private async Task OnSubmitAsync()
		{
			if (DoneNotification.Length > 0) { return; }

			HandleCreateRecipe handleCreateRecipe = new(_dbContext);

			Stream stream = null;
			if (file != null) { stream = file.OpenReadStream(); }

			var createRecipeActionResult = await handleCreateRecipe.DoCreateRecipe(RecipeModel.DishTypeId, stream, RecipeModel.Name);

			if (createRecipeActionResult.Contains("dishtype")) { DishTypeIdWarning = "Druh jedla je povinný údaj!"; }
			if (createRecipeActionResult.Contains("file")) { FileWarning = "Súbor s receptom nebol vybraný!"; }
			if (createRecipeActionResult.Contains("name")) { NameWarning = "Názov receptu je povinný údaj!"; }

			if (createRecipeActionResult.Contains("done")) { DoneNotification = "Nový recept bol pridaný."; }
		}

		private void ResetPropertiesAndFields()
		{
			file = null;
			FileName = string.Empty;
			RecipeModel.Id = 0;
			RecipeModel.Name = string.Empty;

			DishTypeIdWarning = string.Empty;
			FileWarning = string.Empty;
			NameWarning = string.Empty;
			DoneNotification = string.Empty;
		}
	}
}