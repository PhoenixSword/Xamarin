using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Models.Models;

namespace Xamarin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
            MenuPages.Add((int)MenuItemType.MyDishes, new NavigationPage(new DishesPage()));
            MenuPages.Add((int)MenuItemType.Settings, new NavigationPage(new SettingsPage()));
        }

        public async Task NavigateFromMenu(int id)
        {
            if (id == (int)MenuItemType.LogOut)
            {
                Application.Current.Properties["profile"] = null;;
                Application.Current.MainPage = new LoginPage();
                return;
            }
            //if (!MenuPages.ContainsKey(id))
            //{
            //    switch (id)
            //    {
            //        case (int) MenuItemType.MyDishes:
            //            MenuPages.Add(id, new NavigationPage(new DishesPage()));
            //            break;
            //        case (int)MenuItemType.Settings:
            //            MenuPages.Add(id, new NavigationPage(new SettingsPage()));
            //            break;
            //    }
            //}

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}