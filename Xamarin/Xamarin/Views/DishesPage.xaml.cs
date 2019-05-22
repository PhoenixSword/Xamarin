using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Models;
using Xamarin.ViewModels;

namespace Xamarin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class DishesPage
    {
        string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
        DishesViewModel viewModel;
        public DishesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new DishesViewModel();
            DishesList.ItemsSource = viewModel.Dishes;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Dish dish))
                return;
            await Navigation.PushAsync(new NewDishPage(new DishDetailViewModel(dish)));

            DishesList.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewDishPage());
        }


        protected override async void OnAppearing()
        {
            loading.IsVisible = true;
            await viewModel.GetDishes();
            base.OnAppearing();
            loading.IsVisible = false;
        }
    }
}