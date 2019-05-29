using System.Linq;

namespace Xamarin.Models.Models.Mappings
{
    public static class DishMapping
    {
        public static Dish Map(this Dish dish)
        {
            return new Dish
            {
                Id = dish.Id,
                Description = dish.Description,
                Image = dish.Image,
                Ingredients = dish.Ingredients.Select(i=>i.Map()).ToList(),
                Name = dish.Name,
                Sum = dish.Sum

            };
        }
    }
}
