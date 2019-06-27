using System;
using Android.App;
using Xamarin.Forms.Platform.Android;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Droid;
using System.Net;
using Newtonsoft.Json.Linq;
using Xamarin.Views;

[assembly: ExportRenderer(typeof(FacebookPage), typeof(FacebookPageRenderer_Droid))]
namespace Xamarin.Droid
{
    [Obsolete]
    public class FacebookPageRenderer_Droid : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            var activity = Context as Activity;
            var auth = new OAuth2Authenticator
            (
                clientId: "322820598640624",
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html")
            );

            var result = new LoginResult { };
            auth.Completed += (sender, eventArgs) =>
            {
                if (eventArgs.Account == null)
                {
                    new AlertDialog.Builder(Context).SetTitle("Authentication canceled").SetMessage("You didn't completed the authentication process").Show();
                    App.Current.MainPage = new LoginPage();
                    return;
                }
                var userId = "";
                var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=name,picture,cover,birthday"), null, eventArgs.Account);
                var response = request.GetResponseAsync()?.Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var userJson = response.GetResponseText();
                    var jobject = JObject.Parse(userJson);
                    result.LoginState = LoginState.Success;
                    result.FirstName = jobject["name"]?.ToString();
                    userId = jobject["id"]?.ToString();
                    result.ImageUrl = "https://graph.facebook.com/" + userId + "/picture?type=large";
                    result.UserId = userId;
                }
                if (eventArgs.IsAuthenticated)
                {
                    App.SaveToken(userId, result.FirstName + " " + result.LastName, result.ImageUrl);
                    App.SuccessfulLoginAction.Invoke();
                }
            };

            activity.StartActivity(auth.GetUI(activity));
        }
    }
}