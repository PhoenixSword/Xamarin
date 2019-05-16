using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Android.Media;
using Java.Util;
using SQLiteNetExtensions.Attributes;
using Xamarin.Forms;

namespace Xamarin.Models
{
    public class Dish
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        [NotMapped]
        public ImageSource ImageSource { get; set; }
        public string Description { get; set; }

        [OneToMany]
        public List<Ingredient> Ingredients { get; set; }

    }
}