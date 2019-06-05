using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Models.Models;
using Xamarin.ViewModels.Base;
using Xamarin.Views;

namespace Xamarin.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MainPage _page;
        private readonly Dictionary<int, NavigationPage> _menuPages = new Dictionary<int, NavigationPage>();
        public MainViewModel(MainPage page)
        {
            _page = page;
            _menuPages.Add((int)MenuItemType.MyDishes, new NavigationPage(new DishesPage()));
            _menuPages.Add((int)MenuItemType.Settings, new NavigationPage(new SettingsPage()));
        }

        public async Task NavigateFromMenu(int id)
        {
            if (id == (int)MenuItemType.LogOut)
            {
                Application.Current.Properties["id"] = null;
                Application.Current.Properties["name"] = null;
                Application.Current.Properties["image"] = null;
                Application.Current.MainPage = new LoginPage();
                return;
            }

            var newPage = _menuPages[id];

            if (newPage != null && _page.Detail != newPage)
            {
                _page.Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(150);

                _page.IsPresented = false;
            }
        }
    }
}
