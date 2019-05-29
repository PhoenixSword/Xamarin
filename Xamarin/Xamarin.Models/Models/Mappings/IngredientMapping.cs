namespace Xamarin.Models.Models.Mappings
{
    public static class IngredientMapping
    {
        public static Ingredient Map(this Ingredient ingredient)
        {
            return new Ingredient
            {
                Id = ingredient.Id,
                Count = ingredient.Count,
                Name = ingredient.Name,
                Price = ingredient.Price,
                DishId = ingredient.DishId
            };
        }
    }
}
