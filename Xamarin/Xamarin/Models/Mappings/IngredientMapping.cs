using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin.Models.Mappings
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
