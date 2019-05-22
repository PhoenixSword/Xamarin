using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xamarin
{
    public class NumberToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 0;
            try
            {
                result = double.Parse(value.ToString().Replace(",", "."));
            }
            catch (Exception e)
            {
                // ignored
            }

            return result;
        }
    }
}