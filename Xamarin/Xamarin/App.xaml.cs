using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.ViewModels.Base;
using System;
using System.Net;
using Android.Graphics;
using System.IO;
using Plugin.Media.Abstractions;

namespace Xamarin
{
    public partial class App
    {
        public const string Dbfilename = "app.db";
        public static bool isLogin = false;

        private readonly Dictionary<string, string> _orangeStyle = new Dictionary<string, string>
        {
            {"Background", "Cornsilk"},
            {"ItemColor", "Orange"}
        };
        private readonly Dictionary<string, string> _blueStyle = new Dictionary<string, string>
        {
            {"Background", "LightSkyBlue"},
            {"ItemColor", "DodgerBlue"}
        };
        private readonly Dictionary<string, string> _pinkStyle = new Dictionary<string, string>
        {
            {"Background", "LightPink"},
            {"ItemColor", "DeepPink"}
        };
        public App()
        {
            InitializeComponent();
            if (Current.Properties.TryGetValue("Style", out _)) SetStyle(int.Parse(Current.Properties["Style"].ToString()));
            if (!Current.Properties.TryGetValue("id", out _)) Current.Properties["id"] = null;
            if (!Current.Properties.TryGetValue("name", out _)) Current.Properties["name"] = null;
            if (!Current.Properties.TryGetValue("image", out _)) Current.Properties["image"] = null;

            if (Current.Properties["id"] != null)
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new LoginPage();
            }
            MessagingCenter.Subscribe<object, int>(this, MessageKeys.Style, (s, e) =>
            {
                Forms.Device.BeginInvokeOnMainThread(async () => {
                    SetStyle(e);
                    Current.Properties["Style"] = e;
                    await Current.SavePropertiesAsync();
                });

            });
        }

        private void SetStyle(int number)
        {
            switch (number)
            {
                case 0:
                    Resources["Background"] = _orangeStyle["Background"];
                    Resources["ItemColor"] = _orangeStyle["ItemColor"];
                    break;
                case 1:
                    Resources["Background"] = _blueStyle["Background"];
                    Resources["ItemColor"] = _blueStyle["ItemColor"];
                    break;
                case 2:
                    Resources["Background"] = _pinkStyle["Background"];
                    Resources["ItemColor"] = _pinkStyle["ItemColor"];
                    break;
            }
        }
        protected override void OnStart()
        {
            AppCenter.Start("android=12f20db8-63a7-4ffb-93f4-f270dd16c130;",  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            if(isLogin)
                MainPage = new MainPage();
            else
                MainPage = new LoginPage();
        }

        public static bool IsLoggedIn
        {
            get { return Current.Properties.TryGetValue("id", out _); }
        }

        public static void SaveToken(string id, string name, string image)
        {
            Current.Properties["id"] = id;
            Current.Properties["name"] = name;
            if(image != null)
            using (var client = new WebClient())
            {
                var content = client.DownloadData(image);
                Current.Properties["image"] = content;
            }
        }

        public static Action SuccessfulLoginAction 
        => new Action(() =>
        {
            isLogin = true;
        });
    }
}
