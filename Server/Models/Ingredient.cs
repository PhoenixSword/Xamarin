using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Ingredient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Count { get; set; }
        public double Price { get; set; }

        [ForeignKey("Dish")]
        public string DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
