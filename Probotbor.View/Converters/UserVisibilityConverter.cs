using Probotbor.Core.Models.AccessControl;
using System;
using System.Globalization;
using System.Windows;

namespace Probotbor.View.Converters
{
    public class UserVisibilityConverter : Converter
    {
        public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null || (value is not User user)) return Visibility.Collapsed;
            if (parameter is null || parameter is not UserAccessLevel userAccessLevel) return Visibility.Collapsed;
            if (user.AccessLevel >= userAccessLevel)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }
    }
}
