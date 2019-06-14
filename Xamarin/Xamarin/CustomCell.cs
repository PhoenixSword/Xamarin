using Xamarin.Forms;

namespace Xamarin
{
    public class CustomCell : ViewCell
    {
        public CustomCell()
        {
            var image = new Image();
            StackLayout cellWrapper = new StackLayout();
            StackLayout horizontalLayout = new StackLayout();
            Label left = new Label();

            left.SetBinding(Label.TextProperty, "Title");
            image.SetBinding(Image.SourceProperty, "ImagePath");

            left.FontSize = 35;
            image.WidthRequest = 40;
            image.HeightRequest = 40;
            image.Margin = new Thickness(20, 0);

            horizontalLayout.Orientation = StackOrientation.Horizontal;
            horizontalLayout.Padding = new Thickness(50, 5, 5, 5);
            left.TextColor = Color.Black;
            left.HorizontalOptions = LayoutOptions.Center;

            horizontalLayout.Children.Add(image);
            horizontalLayout.Children.Add(left);
            cellWrapper.Children.Add(horizontalLayout);
            View = cellWrapper;
        }
    }
}
