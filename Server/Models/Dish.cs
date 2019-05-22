using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Dish
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Sum { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }

        public List<Ingredient> Ingredients { get; set; }

    }
}