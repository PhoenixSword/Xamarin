using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xamarin.Forms;

namespace Xamarin.Models.Models
{
    public class Dish
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Sum { get; set; }
        public byte[] Image { get; set; }
        [NotMapped]
        public ImageSource ImageSource { get; set; }
        public string Description { get; set; }

        public string ProfileId { get; set; }

        [OneToMany]
        public List<Ingredient> Ingredients { get; set; }

    }
}