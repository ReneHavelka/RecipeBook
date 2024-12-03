using Application.Common.Interfaces;
using Application.DishTypes.Queries;
using Application.Recipes.EventHandlers;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlazorApp.Components.Pages.Recipes
{
	public partial class CreateOrUpdateRecipe
	{
		IApplicationDbContext _dbContext;
		private string modeName = String.Empty;

		[SupplyParameterFromForm]
		private Recipe RecipeModel { get; set; }

		private IList<DishType> dishTypeList = new List<DishType>();
		private IBrowserFile file;
		private string FileName;

		private string DishTypeIdWarning { get; set; }
		private string FileWarning { get; set; }
		private string NameWarning { get; set; }
		private string DoneNotification { get; set; } = String.Empty;

		[Parameter]
		[SupplyParameterFromQuery]
		public int DishTypeId { get; set; }
		[Parameter]
		[SupplyParameterFromQuery]
		public string RecipeName { get; set; }
		[Parameter]
		[SupplyParameterFromQuery]
		public int RecipeId { get; set; }


		public CreateOrUpdateRecipe(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		protected override async Task OnParametersSetAsync()
		{
			RecipeModel = new();

			GetDishTypes getDishTypes = new(_dbContext);
			dishTypeList = await getDishTypes.GetDishTypeListAsync();

			//Aktualizácia
			if (RecipeId != 0)
			{
				RecipeModel.DishTypeId = DishTypeId;
				RecipeModel.Name = RecipeName;
				modeName = "Aktualizácia receptu";
			}
			else
			//Nový recept
			{
				modeName = "Nový recept";
			}
		}

		private void DishTypeSelected()
		{
			if (RecipeModel.DishTypeId > 0) { DishTypeIdWarning = String.Empty; }
			DoneNotification = String.Empty;
		}

		private void LoadFile(InputFileChangeEventArgs e)
		{
			file = e.File;
			FileWarning = String.Empty;
			FileName = file.Name;
			DoneNotification = String.Empty;
		}

		public void NameEntered()
		{
			NameWarning = String.Empty;
			DoneNotification = String.Empty;
		}

		private async Task OnSubmitAsync()
		{
			if (DoneNotification.Length > 0) { return; }

			Stream stream = null;
			if (file != null) { stream = file.OpenReadStream(); }

			string createRecipeActionResult = String.Empty;

			if (modeName == "Nový recept")
			{
				HandleCreateRecipe handleCreateRecipe = new(_dbContext);
				createRecipeActionResult = await handleCreateRecipe.DoCreateRecipe(RecipeModel.DishTypeId, stream, RecipeModel.Name);
			}
			else
			{
				if (RecipeModel.Name is null || RecipeModel.Name.Trim() == String.Empty) { RecipeModel.Name = RecipeName; }
				HandleUpdateRecipe handleUpdateRecipe = new(_dbContext);
				createRecipeActionResult = await handleUpdateRecipe.DoUpdateRecipe(RecipeId, RecipeModel.DishTypeId, stream, RecipeModel.Name);
			}

			if (createRecipeActionResult.Contains("dishtype")) { DishTypeIdWarning = "Druh jedla je povinný údaj!"; }
			if (createRecipeActionResult.Contains("file")) { FileWarning = "Súbor s receptom nebol vybraný!"; }
			if (createRecipeActionResult.Contains("name")) { NameWarning = "Názov receptu je povinný údaj!"; }
			if (createRecipeActionResult.Contains("recipeCreated")) { DoneNotification = "Nový recept bol pridaný."; }
			if (createRecipeActionResult.Contains("recipeUpdated")) { DoneNotification = "Recept bol uktualizovaný."; }
		}

		private void ResetPropertiesAndFields()
		{
			file = null;
			FileName = String.Empty;
			if (RecipeId == 0) { RecipeModel.Id = 0; }
			RecipeModel.Name = String.Empty;
			DishTypeIdWarning = String.Empty;
			FileWarning = String.Empty;
			NameWarning = String.Empty;
			DoneNotification = String.Empty;
		}
	}
}