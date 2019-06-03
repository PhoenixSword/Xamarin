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
    public partial class RegisterPage
    {
        private DishesService dishesService = new DishesService();
        public RegisterPage()
        {
            InitializeComponent();
           
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var name = Email.Text;
            var password = Password.Text;
            var passwordConfirm = PasswordConfirm.Text;

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

            var register = dishesService.Register(name, password);
            if (register != null)
            {
                Application.Current.Properties["id"] = register.Id;
                Application.Current.Properties["name"] = register.Name;
                Application.Current.Properties["image"] = register.Image;
                await Application.Current.SavePropertiesAsync();
                Application.Current.MainPage = new MainPage();
                MessagingCenter.Send<object, Profile>(this, "Settings", register);
                return;
            }
            DependencyService.Get<IMessage>().LongAlert("Wrong credentials found!");
        }

        private void Login_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}