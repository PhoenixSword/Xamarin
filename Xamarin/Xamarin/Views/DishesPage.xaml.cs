using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Plugin.Connectivity;
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
        private bool firstStart = true;
        DishesViewModel viewModel;
        public DishesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new DishesViewModel();
            DishesList.ItemsSource = viewModel.Dishes;
            MessagingCenter.Subscribe<object, bool>(this, "Connection", (s, e) => {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    connectionLabel.BackgroundColor = e ? Color.Green : Color.Gray;
                    connectionLabel.Text = e ? "Back online" : "No connection";

                    if (connectionLabel.IsVisible && e)
                    {

                        await connectionLabel.FadeTo(0, 2000);
                        Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                        {
                            connectionRow.Height = 0;
                            return false;
                        });
                    }
                    else if(!e)
                    {
                        connectionLabel.IsVisible = true;
                        connectionRow.Height = 20;
                        await connectionLabel.FadeTo(1, 500);
                    }
                    
                });
            });
            var thread = new Thread(Check) {IsBackground = true};
            thread.Start();
        }
        public void Check()
        {
            while (true)
            {
                CrossConnectivity.Current.IsRemoteReachable("192.168.0.141", 8080).ContinueWith(task =>
                    {
                        MessagingCenter.Send<object, bool>(this, "Connection", task.Result);
                    });

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
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
            var result = await viewModel.GetDishes();

            if (result == null)
            {
                await DisplayAlert("Error", "Connection refused", "OK");
            }
        }


        protected override async void OnAppearing()
        {
            if (firstStart)
            {
                loading.IsVisible = true;
                await viewModel.GetDishes();
            }

            viewModel.LoadDishesCommand.Execute(null);
            base.OnAppearing();
            if (firstStart) loading.IsVisible = false;
            firstStart = false;
            //Empty.IsVisible = !viewModel.Dishes.Any();
        }
    }
}