using Xamarin.Forms;

namespace Xamarin.Extension
{
    public static class VisualElementExtensions
    {
        public static async System.Threading.Tasks.Task FadeOut(this VisualElement element, uint duration = 400, Easing easing = null)
        {
            await element.FadeTo(0, duration, easing);
            element.IsVisible = false;
        }

        public static async System.Threading.Tasks.Task FadeIn(this VisualElement element, uint duration = 400, Easing easing = null)
        {
            element.IsVisible = true;
            await element.FadeTo(1, duration, easing);
        }
    }
}
