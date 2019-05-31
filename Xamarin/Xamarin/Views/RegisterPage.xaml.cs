using System;
using System.ComponentModel;
using Xamarin.Forms;
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

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var email = Email.Text;
            var password = Password.Text;
            var passwordConfirm = PasswordConfirm.Text;

            if (email == null || password == null)
            {
                DependencyService.Get<IMessage>().LongAlert("Enter login and password");
                return;
            }

            if (password != passwordConfirm)
            {
                DependencyService.Get<IMessage>().LongAlert("Passwords not match");
                return;
            }

            if (dishesService.Register(email, password))
            {
                Application.Current.Properties["token"] = "test";
                Application.Current.MainPage = new MainPage();
                return;
            }
            DependencyService.Get<IMessage>().LongAlert("Wrong credentials found!");

        }

        private void Login_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage(); ;
        }
    }
}