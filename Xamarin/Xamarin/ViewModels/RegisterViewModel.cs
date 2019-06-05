using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Models.Models;
using Xamarin.Services;
using Xamarin.ViewModels.Base;
using Xamarin.Views;

namespace Xamarin.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly DishesService _dishesService = new DishesService();
        public ICommand RegisterCommand => new Command(async () => await RegisterAsync());
        public ICommand LoginPageCommand => new Command(LoginPage);

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
        private string _passwordConfirm;
        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                if (_passwordConfirm == value || value == null)
                    return;
                _passwordConfirm = value;
                OnPropertyChanged();
            }
        }

        private async Task RegisterAsync()
        {
            var name = Email;
            var password = Password;
            var passwordConfirm = PasswordConfirm;

            if (name == null || password == null)
            {
                DependencyService.Get<IMessage>().LongAlert("Enter login and password");
                return;
            }

            if (password != passwordConfirm)
            {
                DependencyService.Get<IMessage>().LongAlert("Passwords not match");
                return;
            }

            var register = _dishesService.Register(name, password);
            if (register != null)
            {
                Application.Current.Properties["id"] = register.Id;
                Application.Current.Properties["name"] = register.Name;
                Application.Current.Properties["image"] = register.Image;
                Application.Current.MainPage = new MainPage();
                MessagingCenter.Send<object, Profile>(this, MessageKeys.Settings, register);
                await Application.Current.SavePropertiesAsync();
                return;
            }
            DependencyService.Get<IMessage>().LongAlert("Wrong credentials found!");
        }


        private static void LoginPage()
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}