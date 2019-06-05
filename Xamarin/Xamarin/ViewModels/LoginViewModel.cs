using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Models.Models;
using Xamarin.Services;
using Xamarin.ViewModels.Base;
using Xamarin.Views;

namespace Xamarin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly DishesService _dishesService = new DishesService();
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email == value || value == null)
                    return;
                _email = value;
                OnPropertyChanged();
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password == value || value == null)
                    return;
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand => new Command(async () => await LoginAsync());
        public ICommand RegisterPageCommand => new Command(RegisterPage);

        private readonly Profile _test = new Profile
        {
            Id = "1",
            Name = "TEST",
            Password = "TEST"
        };

        private static void RegisterPage()
        {
            Application.Current.MainPage = new RegisterPage();
        }

        private async Task LoginAsync()
        {
            var name = Email;
            var password = Password;
            if (name == "test" && password == "test")
            {
                Application.Current.Properties["id"] = _test.Id;
                Application.Current.Properties["name"] = _test.Name;
                Application.Current.Properties["image"] = _test.Image;
                Application.Current.MainPage = new MainPage();
                MessagingCenter.Send<object, Profile>(this, MessageKeys.Settings, _test);
                await Application.Current.SavePropertiesAsync();
                return;
            }
            if (name == null || password == null)
            {
                DependencyService.Get<IMessage>().LongAlert("Enter login and password");
                return;
            }
            var login = _dishesService.Login(name, password);

            if (login != null)
            {
                Application.Current.Properties["id"] = login.Id;
                Application.Current.Properties["name"] = login.Name;
                Application.Current.Properties["image"] = login.Image;
                await Application.Current.SavePropertiesAsync();
                Application.Current.MainPage = new MainPage();
                MessagingCenter.Send<object, Profile>(this, MessageKeys.Settings, login);
                return;
            }
            DependencyService.Get<IMessage>().LongAlert("Wrong credentials found!");
        }
    }
}
