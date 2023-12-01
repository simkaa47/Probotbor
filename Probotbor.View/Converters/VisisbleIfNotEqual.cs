using System.Globalization;
using System.Windows;
using System;

namespace Probotbor.View.Converters
{
    internal class VisisbleIfNotEqual : Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return Visibility.Collapsed;
            if (value.ToString() == parameter.ToString()) return Visibility.Collapsed;
            return Visibility.Visible;
        }
    }
}
