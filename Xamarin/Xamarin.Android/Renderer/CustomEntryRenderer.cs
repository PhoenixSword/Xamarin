using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Util;
using Xamarin;
using Xamarin.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Xamarin.Android
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
            var _gradientBackground = new GradientDrawable();
            _gradientBackground.SetShape(ShapeType.Rectangle);
            _gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

            // Thickness of the stroke line
            _gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

            // Radius for the curves
            _gradientBackground.SetCornerRadius(
                DpToPixels(this.Context,
                    Convert.ToSingle(view.CornerRadius)));

            // set the background of the label
            Control.SetBackground(_gradientBackground);
            //Control.SetRawInputType(InputTypes.ClassNumber);


        }
        public static float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }
    }
}