using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Models;
using Xamarin.ViewModels;

namespace Xamarin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class NewDishPage : ContentPage
    {
        string dbPath;
        public Dish Dish { get; set; }
        public string Name { get; set; }
        private MediaFile selectedImageFile { get; set; }

        public NewDishPage()
        {
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            Name = "Add Dish";
            InitializeComponent();

            Dish = new Dish
            {
                Name = "Dish name",
                Description = "This is an dish description.",
                Image = null,
                ImageSource = ImageSource.FromFile("EmptyImage.jpg"),
                Ingredients = new List<Ingredient>()
            };
            selectedImage.Source = Dish.ImageSource;
            IngredientsList.HeightRequest = 0;
            BindingContext = this;
        }

        public NewDishPage(DishDetailViewModel dishDetail)
        {
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            var deleteButton = new ToolbarItem {Text = "Delete", Icon = "DeleteIcon.png"};
            Name = "Edit Dish";
            deleteButton.Clicked += Delete_Clicked;
            InitializeComponent();
            Dish = new Dish
            {
                Id = dishDetail.Dish.Id,
                Name = dishDetail.Dish.Name,
                Description = dishDetail.Dish.Description,
                Image = dishDetail.Dish.Image,
                ImageSource = dishDetail.Dish.ImageSource,
                Ingredients = dishDetail.Dish.Ingredients

            };
            IngredientsList.ItemsSource = Dish.Ingredients;
            selectedImage.Source = Dish.ImageSource;
            IngredientsList.HeightRequest = 60 * Dish.Ingredients.Count;
            BindingContext = this;

            ToolbarItems.Add(deleteButton);
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            var dish = Dish;
            if (!string.IsNullOrEmpty(dish.Name))
            {
                using (ApplicationContext db = new ApplicationContext(dbPath))
                {
                    if (string.IsNullOrEmpty(dish.Id))
                        db.Dishes.Add(dish);
                    else
                    {
                        db.Dishes.Update(dish);
                    }
                    db.SaveChanges();
                }
            }
            Navigation.PopAsync();
        }
        private void Delete_Clicked(object sender, EventArgs e)
        {
            var dish = Dish;
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                db.Dishes.Remove(dish);
                db.SaveChanges();
            }
            Navigation.PopAsync();
        }

        async void Handle_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Not supported", "Not supported", "Ok");
                return;
            }
            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };
            selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
            if (selectedImageFile == null)
            {
                return;
            }
            selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
            Dish.Image = GetImageStreamAsBytes(selectedImageFile.GetStream());
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

        private void Add(object sender, EventArgs e)
        {
            Dish.Ingredients.Add(new Ingredient { Name = "", Count = ""});
            IngredientsList.HeightRequest += 60;
            IngredientsList.ItemsSource = null;
            IngredientsList.ItemsSource = Dish.Ingredients;

        }

        private void Remove(Ingredient ingredient)
        {
            Dish.Ingredients.Remove(ingredient);
            IngredientsList.HeightRequest -= 60;
            IngredientsList.ItemsSource = null;
            IngredientsList.ItemsSource = Dish.Ingredients;

        }
        private void Delete_Ingredient(object sender, EventArgs e)
        {
            var button = sender as Image;
            var ingredient = button?.BindingContext as Ingredient;
            Remove(ingredient);

        }
    }
}