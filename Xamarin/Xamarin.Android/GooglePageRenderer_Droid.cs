﻿using System;
using Android.App;
using Xamarin.Forms.Platform.Android;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Droid;
using System.Net;
using Newtonsoft.Json.Linq;
using Xamarin.Views;

[assembly: ExportRenderer(typeof(GooglePage), typeof(GooglePageRenderer_Droid))]
namespace Xamarin.Droid
{
    [Obsolete]
    public class GooglePageRenderer_Droid : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            var activity = Context as Activity;
            var auth = new OAuth2Authenticator
            (
                "340945119693-65r7k3u9peg7414nvle903g2nkhav2da.apps.googleusercontent.com",
                "QLnSLXjvl9CpJmX8MN4_qD0y",
                "email",
                new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                new Uri("com.xamarin.dishes.googleauth:/oauth2redirect"),
                new Uri("https://accounts.google.com/o/oauth2/token"),
                isUsingNativeUI: true
            )
            {
                AllowCancel = true,
            };

            var result = new LoginResult{};

            auth.Completed += (sender, eventArgs) =>
            {
                if (eventArgs.Account == null) return;
                var userId = "";
                var request = new OAuth2Request("GET", new Uri("https://www.googleapis.com/oauth2/v2/userinfo"), null, eventArgs?.Account);
                var response = request.GetResponseAsync().Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var userJson = response.GetResponseText();
                    var jobject = JObject.Parse(userJson);
                    result.LoginState = LoginState.Success;
                    result.Email = jobject["emails"]?["preferred"].ToString();
                    result.FirstName = jobject["first_name"]?.ToString();
                    result.LastName = jobject["last_name"]?.ToString();
                    result.ImageUrl = jobject["picture"]?["data"]?["url"]?.ToString();
                    userId = jobject["id"]?.ToString();
                    result.UserId = userId;
                    result.ImageUrl = $"https://apis.live.net/v5.0/{userId}/picture";
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
