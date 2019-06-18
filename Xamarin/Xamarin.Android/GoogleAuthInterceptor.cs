using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;

namespace Xamarin.Droid
{
    [Activity(Label = "GoogleAuthInterceptor", LaunchMode = LaunchMode.SingleTask)]
    [
        IntentFilter
        (
            actions: new[] { Intent.ActionView },
            Categories = new[]
            {
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
            },
            DataSchemes = new[]
            {
                // First part of the redirect url (Package name)
                "com.xamarin.dishes"
            },
            DataPaths = new[]
            {
                // Second part of the redirect url (Path)
                "/oauth2redirect"
            }
        )
    ]
    public class GoogleAuthInterceptor : Activity
    {
        static public Uri uri { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Android.Net.Uri uri_android = Intent.Data;

            // Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
            Uri uri_netfx = new Uri(uri_android.ToString());
            uri = uri_netfx;
            // Send the URI to the Authenticator for continuation
            GoogleRenderer.Auth.OnPageLoading(uri_netfx);

            var intent = new Intent(this, typeof(MainActivity)).SetFlags(ActivityFlags.ReorderToFront);
            StartActivity(intent);

            Finish();
        }
    }
}