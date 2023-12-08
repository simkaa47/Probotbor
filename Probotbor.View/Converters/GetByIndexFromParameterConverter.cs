using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Probotbor.View.Converters
{
    public class GetByIndexFromParameterConverter : Converter
    {
        public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int index = 0;
            if (value is null) return null;
            if (!(int.TryParse(value.ToString(), out index)) && value is not Enum) return null;
            if (value is Enum @enum)
                index = System.Convert.ToInt32(@enum);
            if (!(parameter is IEnumerable<object> list)) return null;
            if (list.Count() < index + 1) return null;
            return list.ElementAt(index);
        }
    }
}
