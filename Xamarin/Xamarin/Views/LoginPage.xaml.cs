using System;
using Xamarin.ViewModels;

namespace Xamarin.Views
{
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
    }
}