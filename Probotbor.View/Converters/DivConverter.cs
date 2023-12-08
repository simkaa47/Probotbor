using System;
using System.Globalization;

namespace Probotbor.View.Converters
{
    public class DivConverter : Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || parameter is null) return 0;
            var valueStr = value.ToString();
            var parString = parameter.ToString();
            float valueNum = 0;
            float parNum = 0;
            if(!float.TryParse(valueStr, out valueNum))return 0;
            if (!float.TryParse(parString, out parNum)) return 0;
            return valueNum / parNum;   

        }
    }
}
