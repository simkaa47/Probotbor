using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Probotbor.View.Converters
{
    public class AndMultiNotVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 0) return Visibility.Visible;
            var bools = values.Where(value => value != null && value is bool).Select(value => (bool)value);
            if (bools.All(value => value == true))
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
