using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Views;

namespace Xamarin
{
    public partial class App
    {
        public const string Dbfilename = "app.db";

        private readonly Dictionary<string, string> _style1 = new Dictionary<string, string>
        {
            {"Background", "Cornsilk"},
            {"ItemColor", "Orange"}
        };

        private readonly Dictionary<string, string> _style2 = new Dictionary<string, string>
        {
            {"Background", "LightSkyBlue"},
            {"ItemColor", "DodgerBlue"}
        };
        public App()
        {
            InitializeComponent();
            if (Current.Properties.TryGetValue("Style", out var result)) SetStyle(int.Parse(Current.Properties["Style"].ToString()));
           
            if (!Current.Properties.TryGetValue("token", out var resultToken)) Current.Properties["token"] = "";

            MessagingCenter.Subscribe<object, int>(this, "Style", (s, e) =>
            {
                Device.BeginInvokeOnMainThread( () => { SetStyle(e); Current.Properties["Style"] = e; });
            });
            if (!Current.Properties["token"].ToString().Equals(""))
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new LoginPage();
            }
        }

        private void SetStyle(int number)
        {
            switch (number)
            {
                case 0:
                    Resources["Background"] = _style1["Background"];
                    Resources["ItemColor"] = _style1["ItemColor"];
                    break;
                case 1:
                    Resources["Background"] = _style2["Background"];
                    Resources["ItemColor"] = _style2["ItemColor"];
                    break;
            }
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
