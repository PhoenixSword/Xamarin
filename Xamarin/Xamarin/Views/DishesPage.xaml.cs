using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Models.Models;
using Xamarin.ViewModels;

namespace Xamarin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class DishesPage
    {
        private bool _firstStart = true;
        private readonly DishesViewModel _viewModel;
        public DishesPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new DishesViewModel();
            DishesList.ItemsSource = _viewModel.Dishes;
            MessagingCenter.Subscribe<object, bool>(this, "Connection", (s, e) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    ConnectionLabel.BackgroundColor = e ? Color.Green : Color.Gray;
                    ConnectionLabel.Text = e ? "Back online" : "No connection";

                    if (ConnectionLabel.IsVisible && e)
                    {

                        await ConnectionLabel.FadeTo(0, 2000);
                        Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                        {
                            ConnectionRow.Height = 0;
                            return false;
                        });
                    }
                    else if (!e)
                    {
                        ConnectionLabel.IsVisible = true;
                        ConnectionRow.Height = 20;
                        await ConnectionLabel.FadeTo(1, 500);
                    }

                });
            });

            Device.StartTimer(TimeSpan.FromSeconds(4), () =>
            {
                CrossConnectivity.Current.IsRemoteReachable("192.168.0.141", 8080).ContinueWith(task =>
                {
                    MessagingCenter.Send<object, bool>(this, "Connection", task.Result);
                });
                return true;
            });
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

        async void Refresh_Clicked(object sender, EventArgs e)
        {
            await _viewModel.GetDishes();

            //if (result == null)
            //{
            //    await DisplayAlert("Error", "Connection refused", "OK");
            //}
        }


        protected override void OnAppearing()
        {
            if (_firstStart)
            {
                Loading.IsVisible = true;
                Task.Run(()=> _viewModel.GetDishes());
            }

            _viewModel.LoadDishesCommand.Execute(null);
            base.OnAppearing();
            if (!_firstStart) return;
            Loading.IsVisible = false;
            _firstStart = false;
        }
    }
}