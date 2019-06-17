using System;
using Android.App;
using Xamarin.Forms.Platform.Android;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Droid;
using System.Net;
using Newtonsoft.Json.Linq;
using Xamarin.Views;

[assembly: ExportRenderer(typeof(MicrosoftPage), typeof(MicrosoftPageRenderer_Droid))]
namespace Xamarin.Droid
{
    [Obsolete]
    public class MicrosoftPageRenderer_Droid : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            var activity = Context as Activity;
            var auth = new OAuth2Authenticator
            (
               clientId: "8ff5a7d8-30a6-4188-8f56-ed5aadfc0e24",
               scope: "wl.basic, wl.emails, wl.photos",
               authorizeUrl: new Uri("https://login.live.com/oauth20_authorize.srf"),
               redirectUrl: new Uri("https://login.microsoftonline.com/common/oauth2/nativeclient"),
               clientSecret: null,
               accessTokenUrl: new Uri("https://login.live.com/oauth20_token.srf")
            )
            {
                AllowCancel = true
            };

            var result = new LoginResult{};

            auth.Completed += (sender, eventArgs) =>
            {
                if (eventArgs.Account == null) return;
                var userId = "";
                var request = new OAuth2Request("GET", new Uri("https://apis.live.net/v5.0/me"), null, eventArgs.Account);
                var response = request.GetResponseAsync()?.Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var userJson = response.GetResponseText();
                    var jobject = JObject.Parse(userJson);
                    result.LoginState = LoginState.Success;
                    result.Email = jobject["emails"]?["preferred"].ToString();
                    result.FirstName = jobject["first_name"]?.ToString();
                    result.LastName = jobject["last_name"]?.ToString();
                    userId = jobject["id"]?.ToString();
                    result.UserId = userId;
                }
                if (eventArgs.IsAuthenticated)
                {
                    App.SaveToken(userId, result.FirstName + " " + result.LastName, null);
                    App.SuccessfulLoginAction.Invoke();
                }
                else
                {
                    // The user cancelled
                }
            };

            activity.StartActivity(auth.GetUI(activity));
        }
    }
}
