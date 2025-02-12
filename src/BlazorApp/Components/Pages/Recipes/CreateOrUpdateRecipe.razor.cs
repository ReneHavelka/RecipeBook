using Application.Common.Interfaces;
using Application.DishTypes.Queries;
using Application.Recipes.EventHandlers;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

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

			//Aktualiz�cia
			if (RecipeId != 0)
			{
				RecipeModel.DishTypeId = DishTypeId;
				RecipeModel.Name = RecipeName;
				modeName = "Aktualiz�cia receptu";
			}
			else
			//Nov� recept
			{
				modeName = "Nov� recept";
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

			if (RecipeModel.Name.Length > 40) { RecipeModel.Name = RecipeModel.Name.Remove(40); }

			Stream stream = null;
			if (file != null) { stream = file.OpenReadStream(maxAllowedSize: 1048576); }

			string recipeCreateOrUpdateActionResult = String.Empty;

			if (modeName == "Nov� recept")
			{
				HandleCreateRecipe handleCreateRecipe = new(_dbContext);
				recipeCreateOrUpdateActionResult = await handleCreateRecipe.DoCreateRecipe(RecipeModel.DishTypeId, stream, RecipeModel.Name);
			}
			else
			{
				if (RecipeModel.Name is null || RecipeModel.Name.Trim() == String.Empty) { RecipeModel.Name = RecipeName; }
				HandleUpdateRecipe handleUpdateRecipe = new(_dbContext);
				recipeCreateOrUpdateActionResult = await handleUpdateRecipe.DoUpdateRecipe(RecipeId, RecipeModel.DishTypeId, stream, RecipeModel.Name);
			}

			if (recipeCreateOrUpdateActionResult.Contains("dishtype")) { DishTypeIdWarning = "Druh jedla je povinn� �daj!"; }
			if (recipeCreateOrUpdateActionResult.Contains("file")) { FileWarning = "S�bor s receptom nebol vybran�!"; }
			if (recipeCreateOrUpdateActionResult.Contains("name")) { NameWarning = "N�zov receptu je povinn� �daj!"; }
			if (recipeCreateOrUpdateActionResult.Contains("recipeCreated")) { DoneNotification = "Nov� recept bol pridan�."; }
			if (recipeCreateOrUpdateActionResult.Contains("recipeUpdated")) { DoneNotification = "Recept bol uktualizovan�."; }
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