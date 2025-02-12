using Application.Common.Interfaces;
using BlazorApp.Controllers;
using Infrastructure;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components.Pages.Recipes
{
	public partial class PdfViewer
	{

		[Parameter]
		[SupplyParameterFromQuery]
		public int RecipeId { get; set; }
		private string pdfUrl;

		protected override void OnInitialized()
		{
			pdfUrl = $"api/pdf/{RecipeId}";
		}
	}
}
