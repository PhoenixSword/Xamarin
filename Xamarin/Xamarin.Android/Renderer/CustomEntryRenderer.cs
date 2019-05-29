using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Xamarin;
using Xamarin.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Xamarin.Droid.Renderer
{
    [Obsolete]
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null) return;
            var view = (CustomEntry)Element;

            if (!view.IsCurvedCornersEnabled) return;

            // creating gradient drawable for the curved background
            var gradientBackground = new GradientDrawable();
            gradientBackground.SetShape(ShapeType.Rectangle);
            gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

            // Thickness of the stroke line
            gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

            // Radius for the curves
            gradientBackground.SetCornerRadius(
                DpToPixels(Context,
                    Convert.ToSingle(view.CornerRadius)));

            // set the background of the label
            Control.SetBackground(gradientBackground);
            //Control.SetRawInputType(InputTypes.ClassNumber);


        }
        public static float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }
    }
}