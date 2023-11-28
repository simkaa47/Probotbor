using System;
using System.Globalization;

namespace Probotbor.View.Converters
{
    public class AddNumberConverter : Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return 0;
            if (parameter is null) return value;
            float varFloat = 0;
            float parFloat = 0;
            if (!float.TryParse(value.ToString(), out varFloat)) return 0;
            if (!float.TryParse(parameter.ToString(), out parFloat)) return 0;
            return varFloat + parFloat;
        }
    }
}
