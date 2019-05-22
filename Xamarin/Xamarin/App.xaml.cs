using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Models;
using Xamarin.Views;

namespace Xamarin
{
    public partial class App : Application
    {
        public const string DBFILENAME = "app.db";

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

       

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
