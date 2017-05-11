using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CGraph
{
    public class EllipsePositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Point) value - new Vector(2.5, 2.5);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
