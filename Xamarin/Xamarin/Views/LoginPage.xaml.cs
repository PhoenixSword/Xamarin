using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Models.Models;
using Xamarin.Services;

namespace Xamarin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class LoginPage
    {
        private DishesService dishesService = new DishesService();
        private Profile test = new Profile
        {
            Id = "1",
            Name = "TEST",
            Password = "TEST"
        };
        public LoginPage()
        {
            InitializeComponent();
           
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var name = Email.Text;
            var password = Password.Text;
            if (name == "test" && password == "test")
            {
                Application.Current.Properties["id"] = test.Id;
                Application.Current.Properties["name"] = test.Name;
                Application.Current.Properties["image"] = test.Image;
                await Application.Current.SavePropertiesAsync();
                Application.Current.MainPage = new MainPage();
                MessagingCenter.Send<object, Profile>(this, "Settings", test);
                return;
            }
            if (name == null || password == null)
            {
                DependencyService.Get<IMessage>().LongAlert("Enter login and password");
                return;
            }

            var login = dishesService.Login(name, password);

            if (login != null)
            {
                Application.Current.Properties["id"] = login.Id;
                Application.Current.Properties["name"] = login.Name;
                Application.Current.Properties["image"] = login.Image;
                await Application.Current.SavePropertiesAsync();
                Application.Current.MainPage = new MainPage();
                MessagingCenter.Send<object, Profile>(this, "Settings", login);
                return;
            }

            DependencyService.Get<IMessage>().LongAlert("Wrong credentials found!");

        }

        private void Register_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegisterPage();
        }
    }
}