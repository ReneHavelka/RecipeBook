namespace Domain.Entities
{
	public class Recipe
	{
		public int Id { get; set; }
		public int DishTypeId { get; set; }
		public string Name { get; set; }
		public byte[] RecipeInstr { get; set; }
	}
}
