using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;
using Xamarin.Models;
using Xamarin.Services;

namespace Xamarin.ViewModels
{
    public class NewDishViewModel : BaseViewModel
    {
        readonly DishesService dishesService = new DishesService();
        public Dish Dish { get; set; }
        public string Name { get; set; }

        public NewDishViewModel()
        {
            Title = "Add Dish";
            Dish = new Dish
            {
                Name = "Dish name",
                Description = "This is an dish description.",
                Image = null,
                Sum = 0,
                ImageSource = ImageSource.FromFile("EmptyImage.jpg"),
                Ingredients = new List<Ingredient>()
            };
        }

        public NewDishViewModel(DishDetailViewModel dishDetail)
        {
            Title = "Edit Dish";
            Dish = new Dish
            {
                Id = dishDetail.Dish.Id,
                Name = dishDetail.Dish.Name,
                Description = dishDetail.Dish.Description,
                Image = dishDetail.Dish.Image,
                Sum = dishDetail.Dish.Sum,
                ImageSource = dishDetail.Dish.ImageSource,
                Ingredients = dishDetail.Dish.Ingredients
            };
        }

        public void SelectImage(Stream stream)
        {
            Dish.Image = GetImageStreamAsBytes(stream);
        }

        public byte[] GetImageStreamAsBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public void Save(double sum)
        {
            Dish.Sum = sum;
            if (string.IsNullOrEmpty(Dish.Name)) return;
            if (string.IsNullOrEmpty(Dish.Id))
                db.Dishes.Add(Dish);
            else
            {
                var listIngredients = db.Ingredients.Where(t => t.DishId == Dish.Id).Except(Dish.Ingredients);

                foreach (var item in listIngredients)
                {
                    db.Ingredients.Remove(item);
                }
                db.Update(Dish);
            }
            db.SaveChanges();
            dishesService.UpdateDishes(Dish);
        }

        public void Delete()
        {
            db.Dishes.Remove(Dish);
            db.SaveChanges();
            dishesService.DeleteDishes(Dish.Id);
        }

        public void Add()
        {
            Dish.Ingredients.Add(new Ingredient { Name = "", Count = "0", Price = 0 });
        }

        public void Remove(Ingredient ingredient)
        {
            Dish.Ingredients.Remove(ingredient);
        }
    }
}