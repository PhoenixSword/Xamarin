using Xamarin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Android.App;
using Java.Lang;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.ViewModels;
using static Xamarin.Forms.Device;
using Application = Xamarin.Forms.Application;

namespace Xamarin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MenuPage
    {
        MainPage RootPage => Application.Current.MainPage as MainPage;
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();
            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.MyDishes, Title="My Dishes" },
                new HomeMenuItem {Id = MenuItemType.Settings, Title="Settings" }
            };

            ListViewMenu.ItemsSource = menuItems;
            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;
               
                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };

            MessagingCenter.Subscribe<object, Profile>(this, "Settings", (s, profile) => {
                selectedImage.Source = ImageSource.FromStream(() => new MemoryStream(profile.Image));
                Name.Text = profile.Name;
            });

        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Exit? ", "Do you really want to exit the application?", "Yes", "No");
            if (result)
            {
                JavaSystem.Exit(0);
            }
        }
    }
}