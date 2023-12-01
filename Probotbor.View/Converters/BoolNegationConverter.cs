using System;
using System.Globalization;

namespace Probotbor.View.Converters
{
    public class BoolNegationConverter:Converter
    {
        public override object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || !(value is bool condition)) return null;
            return !condition;
        }
    }
}
