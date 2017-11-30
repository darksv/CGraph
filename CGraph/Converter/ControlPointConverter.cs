using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace CGraph.Converter
{
    public class ControlPointConverter : IMultiValueConverter
    {
        private const double CurvatureFactor = 0.01;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var points = values.Cast<Point>().ToArray();
            if (points.Length != 2)
            {
                return null;
            }

            var p1 = points[0];
            var p2 = points[1];

            var v = p2 - p1;
            var u = new Vector(-v.Y, v.X);
            u.Normalize();
            var offsetDistance = CurvatureFactor * v.Length;
            return p1 + 0.5 * v + offsetDistance * u;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
