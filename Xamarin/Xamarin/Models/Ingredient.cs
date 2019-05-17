using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Xamarin.Models
{
    public class Ingredient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Count { get; set; }
        public string Price { get; set; }

        [ForeignKey(typeof(Dish))]
        public string DishId { get; set; }
        [ManyToOne]
        public Dish Dish { get; set; }
    }
}
