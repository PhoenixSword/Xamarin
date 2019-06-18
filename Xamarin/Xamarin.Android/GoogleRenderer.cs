using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Android.App;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Authentication;
using Xamarin.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.ViewModels;
using Xamarin.Views;
using Xamarin_GoogleAuth.Services;
using Application = Xamarin.Forms.Application;
using Button = Xamarin.Forms.PlatformConfiguration.AndroidSpecific.Button;

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

            //SetContentView(Resource.Layout.Main);

            Auth = new GoogleAuthenticator(Configuration.ClientId, Configuration.Scope, Configuration.RedirectUrl, this);

            //var googleLoginButton = FindViewById<Button>(Resource.Id.googleLoginButton);
            //googleLoginButton.Click += OnGoogleLoginButtonClicked;
            OnGoogleLoginButtonClicked(null, null);
        }


        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            var activity = this.Context as Activity;
            // Display the activity handling the authentication
            var authenticator = Auth.GetAuthenticator();
            activity?.StartActivity(authenticator.GetUI(activity));

            //var intent = authenticator.GetUI(this);
            //StartActivity(intent);
        }

        public async void OnAuthenticationCompleted(GoogleOAuthToken token)
        {
            HttpClient client = new HttpClient();
            var responseString = await client.GetStringAsync("https://www.googleapis.com/plus/v1/people/me?access_token=" + token.AccessToken);
            var jobject = JObject.Parse(responseString);
            var name = jobject["displayName"].ToString() != "" ? jobject["displayName"].ToString() : jobject["emails"]?[0]?["value"].ToString();
            App.SaveToken(jobject["id"]?.ToString(), name, jobject["image"]?["url"].ToString().Replace("s50", "s255"));
            Application.Current.MainPage = new MainPage();
        }

        public void OnAuthenticationCanceled()
        {
            //new AlertDialog.Builder(this).SetTitle("Authentication canceled").SetMessage("You didn't completed the authentication process").Show();
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
            //new AlertDialog.Builder(this).SetTitle(message).SetMessage(exception?.ToString()).Show();
        }
    }
}