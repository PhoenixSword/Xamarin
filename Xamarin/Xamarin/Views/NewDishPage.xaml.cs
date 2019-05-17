using System;
using System.ComponentModel;
using System.Threading.Tasks;
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
    public partial class NewDishPage
    {
        private readonly NewDishViewModel viewModel;
        private MediaFile selectedImageFile { get; set; }

        public NewDishPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new NewDishViewModel();

            selectedImage.Source = viewModel.Dish.ImageSource;
            IngredientsList.HeightRequest = 0;
        }

        public NewDishPage(DishDetailViewModel dishDetail)
        {
            InitializeComponent();
            BindingContext = viewModel = new NewDishViewModel(dishDetail);

            var deleteButton = new ToolbarItem {Text = "Delete", Icon = "DeleteIcon.png"};
            deleteButton.Clicked += Delete_Clicked;
            ToolbarItems.Add(deleteButton);

            IngredientsList.ItemsSource = viewModel.Dish.Ingredients;
            selectedImage.Source = viewModel.Dish.ImageSource;
            IngredientsList.HeightRequest = 60 * viewModel.Dish.Ingredients.Count;

        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                var sum = 0;
                foreach (var item in viewModel.Dish.Ingredients)
                {
                    sum += int.Parse(item.Price);
                }

                viewModel.Save(sum);
            }
            catch (Exception exception)
            {
                DisplayAlert("Error", exception.Message, "OK");
                return;
            }
            Navigation.PopAsync();
        }
        private void Delete_Clicked(object sender, EventArgs e)
        {
            viewModel.Delete();
            Navigation.PopAsync();
        }

        private async void Image_Clicked(object sender, EventArgs e)
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

            if (selectedImageFile == null) return;
            viewModel.SelectImage(selectedImageFile.GetStream());

            selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
        }


        private async void Add(object sender, EventArgs e)
        {
            viewModel.Add();
            IngredientsList.HeightRequest += 60;
            IngredientsList.ItemsSource = null;
            IngredientsList.ItemsSource = viewModel.Dish.Ingredients;
            await scrollView.ScrollToAsync(stackLayout, ScrollToPosition.End, true);
        }

        private void Delete_Ingredient(object sender, EventArgs e)
        {
            var button = sender as Image;
            var ingredient = button?.BindingContext as Ingredient;

            viewModel.Remove(ingredient);
            IngredientsList.HeightRequest -= 60;
            IngredientsList.ItemsSource = null;
            IngredientsList.ItemsSource = viewModel.Dish.Ingredients;

        }
    }
}