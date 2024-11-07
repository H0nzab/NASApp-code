using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NASApp
{
    public class HazardousToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isHazardous = (bool)value;
            double diameter = double.Parse(parameter.ToString(), CultureInfo.InvariantCulture);
            double factor = Math.Min(diameter / 10.0, 1.0);

            if (isHazardous)
            {
                return new SolidColorBrush(Color.FromRgb(255, (byte)(255 - 255 * factor), (byte)(255 - 255 * factor)));
            }
            else
            {
                return new SolidColorBrush(Color.FromRgb((byte)(255 - 255 * factor), 255, (byte)(255 - 255 * factor)));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
