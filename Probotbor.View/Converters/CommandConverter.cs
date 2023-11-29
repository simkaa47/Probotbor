using Castle.Core.Logging;
using System;
using System.Globalization;
using System.Windows.Input;

namespace Probotbor.View.Converters
{
    class CommandConverter:Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICommand command)
                return command;
            else return null;
        }
    }
}
