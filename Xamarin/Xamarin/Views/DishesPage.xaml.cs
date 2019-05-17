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
        DishesViewModel viewModel;
        public DishesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new DishesViewModel();
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


        protected override void OnAppearing()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                var list = db.Dishes.Select(i => new Dish
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Image = i.Image,
                    Sum = i.Sum,
                    ImageSource = i.Image != null ? ImageSource.FromStream(() => new MemoryStream(i.Image)) : ImageSource.FromFile("EmptyImage.jpg"),
                    Ingredients = i.Ingredients
                    
                }).ToList();
                DishesList.ItemsSource = list;
            }
            
            base.OnAppearing();
        }
    }
}