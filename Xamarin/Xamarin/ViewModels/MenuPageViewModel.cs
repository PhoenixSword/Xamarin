using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Models.Models;
using Xamarin.ViewModels.Base;
using Xamarin.Views;

namespace Xamarin.ViewModels
{
    public class MenuPageViewModel : BaseViewModel
    {
        private readonly Page _page;
        private ObservableCollection<HomeMenuItem> _menuItems;
        public ObservableCollection<HomeMenuItem> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _profileImage;
        public ImageSource ProfileImage
        {
            get => _profileImage;
            set
            {
                if (_profileImage == value || value == null)
                    return;
                _profileImage = value;
                OnPropertyChanged();
            }
        }
        private string _profileName;
        public string ProfileName
        {
            get => _profileName;
            set
            {
                if (_profileName == value || value == null)
                    return;
                _profileName = value;
                OnPropertyChanged();
            }
        }

        private HomeMenuItem _itemSelected;
        public HomeMenuItem ItemSelected
        {
            get => _itemSelected;
            set
            {
                if (_itemSelected == value || value == null)
                    return;
                _itemSelected = value;
                var id = (int)_itemSelected.Id;
                OnPropertyChanged();
                RootPage?.ViewModel.NavigateFromMenu(id);
            }
        }
        MainPage RootPage => Application.Current.MainPage as MainPage;
        public ICommand CloseAppCommand => new Command(async () => await CloseAppAsync());
        public MenuPageViewModel(Page page)
        {
            _page = page;

            _menuItems = new ObservableCollection<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.MyDishes, Title = "My Dishes", ImagePath = "MyDishes.png"},
                new HomeMenuItem {Id = MenuItemType.Settings, Title = "Settings", ImagePath = "Settings.png"},
                new HomeMenuItem {Id = MenuItemType.LogOut, Title = "Logout", ImagePath = "Logout.png"}
            };
            MessagingCenter.Subscribe<object, Profile>(this, MessageKeys.Settings, (s, profile) =>
            {
                ProfileImage = profile.Image != null
                    ? ImageSource.FromStream(() => new MemoryStream(profile.Image))
                    : ImageSource.FromFile("Profile.jpg");
                ProfileName = profile.Name;
            });
        }

        private async Task CloseAppAsync()
        {
            var result = await _page.DisplayAlert("Exit? ", "Do you really want to exit the application?", "Yes", "No");
            if (!result) return;
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
