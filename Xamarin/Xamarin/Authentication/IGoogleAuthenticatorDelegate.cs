using System;

namespace Xamarin.Authentication
{
    public interface IGoogleAuthenticationDelegate
    {
        void OnAuthenticationCompleted(string token);
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCanceled();
    }
}