using SQLiteNetExtensions.Attributes;

namespace Xamarin.Models.Models
{
    public class Ingredient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Count { get; set; }
        public double Price { get; set; }

        [ForeignKey(typeof(Dish))]
        public string DishId { get; set; }

        [ManyToOne]
        public Dish Dish { get; set; }
    }
}
