using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Models.Models;
using Xamarin.ViewModels;

namespace Xamarin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();
           
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Application.Current.Properties["token"] = "test";
            Application.Current.MainPage = new MainPage();
        }
    }
}