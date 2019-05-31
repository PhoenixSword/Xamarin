using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Views;

namespace Xamarin
{
    public partial class App
    {
        public const string Dbfilename = "app.db";

        private readonly Dictionary<string, string> OrangeStyle = new Dictionary<string, string>
        {
            {"Background", "Cornsilk"},
            {"ItemColor", "Orange"}
        };

        private readonly Dictionary<string, string> BlueStyle = new Dictionary<string, string>
        {
            {"Background", "LightSkyBlue"},
            {"ItemColor", "DodgerBlue"}
        };
        private readonly Dictionary<string, string> PinkStyle = new Dictionary<string, string>
        {
            {"Background", "LightPink"},
            {"ItemColor", "DeepPink"}
        };
        public App()
        {
            InitializeComponent();
            if (Current.Properties.TryGetValue("Style", out var result)) SetStyle(int.Parse(Current.Properties["Style"].ToString()));
           
            if (!Current.Properties.TryGetValue("profile", out var resultToken)) Current.Properties["profile"] = null;

            MessagingCenter.Subscribe<object, int>(this, "Style", (s, e) =>
            {
                Device.BeginInvokeOnMainThread( () => { SetStyle(e); Current.Properties["Style"] = e; });
            });
            if (Current.Properties["profile"] != null)
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
                    Resources["Background"] = OrangeStyle["Background"];
                    Resources["ItemColor"] = OrangeStyle["ItemColor"];
                    break;
                case 1:
                    Resources["Background"] = BlueStyle["Background"];
                    Resources["ItemColor"] = BlueStyle["ItemColor"];
                    break;
                case 2:
                    Resources["Background"] = PinkStyle["Background"];
                    Resources["ItemColor"] = PinkStyle["ItemColor"];
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
