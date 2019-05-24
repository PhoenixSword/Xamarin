using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Models;
using Xamarin.Views;

namespace Xamarin
{
    public partial class App : Application
    {
        public const string DBFILENAME = "app.db";
        public bool isReachable;

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        public async void Check()
        {
            isReachable = await CrossConnectivity.Current.IsReachable("http://192.168.0.141:8080");
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
