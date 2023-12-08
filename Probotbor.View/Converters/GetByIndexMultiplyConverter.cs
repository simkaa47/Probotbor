using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Probotbor.View.Converters
{
    class GetByIndexMultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int index = 0;
            if (values.Length < 2) return null;
            if (values[0] is null) return null;
            if (!(values[1] is IEnumerable<object> list)) return null;
            if (!int.TryParse(values[0].ToString(), out index)) return null;
            if (list.Count() < index + 1) return null;
            return list.ElementAt(index);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
