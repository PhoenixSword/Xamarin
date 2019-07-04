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
        private readonly ProfileService _profileService = new ProfileService();
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

        public ICommand LoginCommand => new Command(Login);
        public ICommand MicrosoftCommand => new Command(Microsoft);
        public ICommand GoogleCommand => new Command(Google);
        public ICommand FacebookCommand => new Command(Facebook);
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

        private static void Google()
        {
            Application.Current.MainPage = new GooglePage();
        }
        private static void Facebook()
        {
            Application.Current.MainPage = new FacebookPage();
        }
        private static void Microsoft()
        {
            Application.Current.MainPage = new MicrosoftPage();
        }
        private void Login()
        { 
            var name = Email;
            var password = Password;
            if (name == "test" && password == "test")
            {
                MessagingCenter.Send<object, Profile>(this, MessageKeys.Settings, _test);
                Application.Current.MainPage = new MainPage();
                return;
            }
            if (name == null || password == null)
            {
                DependencyService.Get<IMessage>().LongAlert("Enter login and password");
                return;
            }
            var login = _profileService.Login(name, password);

            if (login != null)
            {
                Application.Current.Properties["id"] = login.Id;
                Application.Current.Properties["name"] = login.Name;
                Application.Current.Properties["image"] = login.Image;
                MessagingCenter.Send<object, Profile>(this, MessageKeys.Settings, login);
                Application.Current.MainPage = new MainPage();
                return;
            }
            DependencyService.Get<IMessage>().LongAlert("Wrong credentials found!");
        }

    }
}
