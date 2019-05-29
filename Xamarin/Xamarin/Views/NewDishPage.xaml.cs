using System;
using System.ComponentModel;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Models.Models;
using Xamarin.ViewModels;

namespace Xamarin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class NewDishPage
    {
        private readonly NewDishViewModel _viewModel;
        private MediaFile SelectedImageFile { get; set; }

        public NewDishPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new NewDishViewModel();

            SelectedImage.Source = _viewModel.Dish.ImageSource;
            IngredientsList.HeightRequest = 0;
        }

        public NewDishPage(DishDetailViewModel dishDetail)
        {
            InitializeComponent();
            BindingContext = _viewModel = new NewDishViewModel(dishDetail);

            var deleteButton = new ToolbarItem {Text = "Delete", IconImageSource = "DeleteIcon.png"};
            deleteButton.Clicked += Delete_Clicked;
            ToolbarItems.Add(deleteButton);

            IngredientsList.ItemsSource = _viewModel.Dish.Ingredients;
            SelectedImage.Source = _viewModel.Dish.ImageSource;
            IngredientsList.HeightRequest = 60 * _viewModel.Dish.Ingredients.Count;

        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            double sum = 0;
            foreach (var item in _viewModel.Dish.Ingredients)
            {
                sum += item.Price;
            }

            _viewModel.Save(sum);

            Navigation.PopAsync();
        }
        private async void Delete_Clicked(object sender, EventArgs e)
        {
            _viewModel.Delete();
            await Navigation.PopAsync();
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

            SelectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (SelectedImageFile == null) return;
            _viewModel.SelectImage(SelectedImageFile.GetStream());

            SelectedImage.Source = ImageSource.FromStream(() => SelectedImageFile.GetStream());
        }


        private async void Add(object sender, EventArgs e)
        {
            _viewModel.Add();
            IngredientsList.HeightRequest += 60;
            IngredientsList.ItemsSource = null;
            IngredientsList.ItemsSource = _viewModel.Dish.Ingredients;
            await ScrollView.ScrollToAsync(StackLayout, ScrollToPosition.End, true);
        }

        private void Delete_Ingredient(object sender, EventArgs e)
        {
            var button = sender as Image;
            var ingredient = button?.BindingContext as Ingredient;

            _viewModel.Remove(ingredient);
            IngredientsList.HeightRequest -= 60;
            IngredientsList.ItemsSource = null;
            IngredientsList.ItemsSource = _viewModel.Dish.Ingredients;

        }
    }
}