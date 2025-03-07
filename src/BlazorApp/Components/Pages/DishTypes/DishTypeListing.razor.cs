using Application.Common.Interfaces;
using Application.DishTypes.Commands;
using Application.DishTypes.Queries;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components.Pages.DishTypes
{
	public partial class DishTypeListing
	{
		int dishTypeIdOnDragStart;

		IApplicationDbContext _dbContext;
		[SupplyParameterFromForm]
		IList<DishType> DishTypeListModel { get; set; } = new List<DishType>();
		[SupplyParameterFromForm]
		int? FirstPositionInOrder { get; set; }
		[SupplyParameterFromForm]
		int? LastPositionInOrder { get; set; }

		[SupplyParameterFromForm]
		IDictionary<int, bool> DishTypesToDelete { get; set; }
		[SupplyParameterFromForm]
		IList<DishType> NewDishTypeListModel { get; set; } = new List<DishType>();
		private string DoneNotification { get; set; }

		[Inject]
		NavigationManager Navigation { get; set; }

		public DishTypeListing(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		protected async override Task OnParametersSetAsync()
		{
			GetDishTypes getDishTypes = new(_dbContext);
			DishTypeListModel = await getDishTypes.GetDishTypeListAsync();

			FirstPositionInOrder = DishTypeListModel.Min(x => x.Order);
			LastPositionInOrder = DishTypeListModel.Max(x => x.Order);

			NewDishTypeListModel.Add(new DishType() { Name = String.Empty });

			DishTypesToDelete = DishTypeListModel.ToDictionary(x => x.Id, y => false);
		}

		private async Task OnSubmitAsync()
		{
			LastPositionInOrder = DishTypeListModel.Max(x => x.Order);

			foreach (var newDishType in NewDishTypeListModel)
			{
				newDishType.Name = newDishType.Name.Trim();
				newDishType.Order = ++LastPositionInOrder;
				++LastPositionInOrder;
			}

			var updateDishType = new UpdateDishType(_dbContext);

			DoneNotification = "Zmeny sa ukladaj�.";

			await updateDishType.AddDishTypes(NewDishTypeListModel);
			await updateDishType.DoUpdateDishTypesAsync(DishTypeListModel);

			Thread.Sleep(1000);

			Navigation.Refresh(true);
		}

		private void NewDishTypeChange(DishType currentNewDishType)
		{
			currentNewDishType.Name = currentNewDishType.Name.Trim();
			var lastNewDishType = NewDishTypeListModel.Last();

			if (lastNewDishType.Name.Trim() != String.Empty)
			{
				NewDishTypeListModel.Add(new DishType { Name = String.Empty });
			}
			else if (lastNewDishType != currentNewDishType && currentNewDishType.Name == String.Empty)
			{
				NewDishTypeListModel.Remove(currentNewDishType);
			}

			StateHasChanged();
		}

		private void DishTypePositionUp(int dishTypeId)
		{
			FirstPositionInOrder = DishTypeListModel.Min(x => x.Order);

			var dishType = DishTypeListModel.FirstOrDefault(x => x.Id == dishTypeId);

			if (dishType.Order == FirstPositionInOrder) { return; }

			var maxOfPrecedingDishTypePositions = DishTypeListModel.Where(x => x.Order < dishType.Order).Select(x => x.Order).Max();
			var precedingDishType = DishTypeListModel.FirstOrDefault(x => x.Order == maxOfPrecedingDishTypePositions);
			var positionToChange = dishType.Order;
			dishType.Order = precedingDishType.Order;
			precedingDishType.Order = positionToChange;

			var lastPositionInOrder = DishTypeListModel.Max(x => x.Order);
			DishTypeListModel = DishTypeListModel.OrderBy(x => x.Id == 0 ? lastPositionInOrder + 1 : x.Order).ToList();

			StateHasChanged();
		}

		private void DishTypePositionDown(int dishTypeId)
		{
			var lastPositionInOrder = DishTypeListModel.Max(x => x.Order);

			var dishType = DishTypeListModel.FirstOrDefault(x => x.Id == dishTypeId);

			if (dishType.Order == lastPositionInOrder) { return; }

			var minOfFollowingDishTypePositions = DishTypeListModel.Where(x => x.Order > dishType.Order).Select(x => x.Order).Min();
			var followingDishType = DishTypeListModel.FirstOrDefault(x => x.Order == minOfFollowingDishTypePositions);
			var positionToChange = dishType.Order;
			dishType.Order = followingDishType.Order;
			followingDishType.Order = positionToChange;

			DishTypeListModel = DishTypeListModel.OrderBy(x => x.Id == 0 ? lastPositionInOrder + 1 : x.Order).ToList();

			StateHasChanged();
		}

		private void NewDishTypeToDelete(DishType currentNewDishType)
		{
			currentNewDishType.Name = String.Empty;
			NewDishTypeChange(currentNewDishType);

			StateHasChanged();
		}

		private async Task ToDelete()
		{
			var dishTypeIds = DishTypesToDelete.Where(x => x.Value == true).Select(x => x.Key);

			var deleteRecipe = new DeleteDishType(_dbContext);
			await deleteRecipe.DoDeleteDishType(dishTypeIds);

			Navigation.Refresh(true);
		}

		private void HandleOnDragStart(int dishTypeIdOnDragStart)
		{
			this.dishTypeIdOnDragStart = dishTypeIdOnDragStart;
		}

		private void HandleOnDragOver(int dishTypeIdOnDragOver)
		{
			if (dishTypeIdOnDragStart == dishTypeIdOnDragOver) { return;}

			var lastPositionInOrder = DishTypeListModel.Max(x => x.Order);

			var dishTypeOnDragStart = DishTypeListModel.FirstOrDefault(x => x.Id == dishTypeIdOnDragStart);
			var onDragStartOrder = dishTypeOnDragStart.Order;
			var dishTypeOnDragOver = DishTypeListModel.FirstOrDefault(x => x.Id == dishTypeIdOnDragOver);

			dishTypeOnDragStart.Order = dishTypeOnDragOver.Order;
			dishTypeOnDragOver.Order = onDragStartOrder;

			DishTypeListModel = DishTypeListModel.OrderBy(x => x.Id == 0 ? lastPositionInOrder + 1 : x.Order).ToList();

			StateHasChanged();



		}
	}
}