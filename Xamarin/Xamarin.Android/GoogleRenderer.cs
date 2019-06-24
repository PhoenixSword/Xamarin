using System;
using System.Net.Http;
using Android.App;
using Newtonsoft.Json.Linq;
using Xamarin.Authentication;
using Xamarin.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Views;
using Application = Xamarin.Forms.Application;

[assembly: ExportRenderer(typeof(GooglePage), typeof(GoogleRenderer))]
namespace Xamarin.Droid
{
    [Obsolete]
    public class GoogleRenderer : PageRenderer, IGoogleAuthenticationDelegate
    {
        public static GoogleAuthenticator Auth;
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            Auth = new GoogleAuthenticator("427412991262-llkb4dne249tpnmgu6k5789et6vesejo.apps.googleusercontent.com", "email", "com.xamarin.dishes:/oauth2redirect", this);
            OnGoogleLoginButtonClicked(null, null);
        }


        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            var activity = this.Context as Activity;
            var authenticator = Auth.GetAuthenticator();
            activity?.StartActivity(authenticator.GetUI(activity));
        }

        public async void OnAuthenticationCompleted(string token)
        {
            HttpClient client = new HttpClient();
            var responseString = await client.GetStringAsync("https://www.googleapis.com/plus/v1/people/me?access_token=" + token);
            var jobject = JObject.Parse(responseString);
            var name = jobject["displayName"].ToString() != "" ? jobject["displayName"].ToString() : jobject["emails"]?[0]?["value"].ToString();
            App.SaveToken(jobject["id"]?.ToString(), name, jobject["image"]?["url"].ToString().Replace("s50", "s255"));
            Application.Current.MainPage = new MainPage();
        }

        public void OnAuthenticationCanceled()
        {
            new AlertDialog.Builder(Context).SetTitle("Authentication canceled").SetMessage("You didn't completed the authentication process").Show();
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
            new AlertDialog.Builder(Context).SetTitle(message).SetMessage(exception?.ToString()).Show();
        }
    }
}