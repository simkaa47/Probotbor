using Probotbor.Infrastructure;
using Probotbor.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Probotbor.Views

{
    #region Математические операции

    #region Операция сложения
    public class AddConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return int.Parse(value.ToString()) + int.Parse((parameter ?? 0).ToString());
            }
            catch (Exception)
            {

                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Операция вычитания
    public class SubConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return int.Parse(value.ToString()) - int.Parse((parameter ?? 0).ToString());
            }
            catch (Exception)
            {

                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Операция умножения
    public class MulConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return int.Parse(value.ToString()) * int.Parse((parameter ?? 0).ToString());
            }
            catch (Exception)
            {

                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Операция деления
    public class DivConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                return float.Parse(value.ToString()) / float.Parse((parameter ?? 1).ToString());
            }
            catch (Exception)
            {

                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #endregion

    public class SomeConverter : IValueConverter    
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable collection = value as IEnumerable;
            int num = 0;
            foreach (var item in collection)
            {

                ListBoxItem listBoxItem = item as ListBoxItem;
                if (listBoxItem != null)
                {
                    if (listBoxItem.Visibility != Visibility.Collapsed) num++;
                }
            }
            return num;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GetHeight: IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float num = float.Parse(value.ToString());
            float height = float.Parse(parameter.ToString());
            
            return height / ((int)Math.Sqrt(num));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class GetWidth : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float num = float.Parse(value.ToString());
            float width = float.Parse(parameter.ToString());
            return width / ((int)Math.Sqrt(num) + num % (int)Math.Sqrt(num));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #region Поиск в строке слова "ошибк" и если нашли, то врзвращаем цвет "красный"
    public class FindTextError : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = value != null ? value.ToString() : "";
            int index = stringValue.ToLower().IndexOf("ошибк");
            if (index >= 0) return Brushes.Tomato;

            return parameter ?? Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Конвертер "Видимость если value  = true
    public class IndicatorVisible : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == false) return Visibility.Collapsed;
            else return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region "Видимость, если value = parameter"
    public class VisibleIfEqual : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) return Visibility.Collapsed;
            try
            {

                if (value.ToString() == parameter.ToString()) return Visibility.Visible;
                return Visibility.Collapsed;

            }
            catch (Exception)
            {

                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Невидимость, если value > parameter
    public class UnVisibleIfGreater : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) return Visibility.Visible;
            try
            {

                float floatParam = float.Parse(parameter.ToString());
                float floatValue = float.Parse(value.ToString());
                if (floatValue > floatParam) return Visibility.Collapsed;
                return Visibility.Visible;

            }
            catch (Exception)
            {

                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Невидимость, если value < parameter
    public class UnVisibleIfLess : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (parameter == null) return Visibility.Collapsed;
            if (value == null) return Visibility.Collapsed;
            try
            {

                float floatParam = float.Parse(parameter.ToString());
                float floatValue = float.Parse(value.ToString());
                if (floatValue < floatParam) return Visibility.Collapsed;
                return Visibility.Visible;

            }
            catch (Exception)
            {

                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Невидимость, если value = true
    public class IndicatorUnVisible : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            if (parameter == null)
            {
                if ((bool)value == true) return Visibility.Collapsed;
                else return Visibility.Visible;
            }
            else if (parameter.ToString() != value.ToString()) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    } 
    #endregion
    public class ColorConverterIndicator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true) return Brushes.Aqua;
            else return 0x00262626;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    } 
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "НЕТ СВЯЗИ С ПЛК";

            if (value.GetType() == typeof(float))
            {
                var secondsFloat = (float)value;
                int seconds = (int)secondsFloat;
                string time = "";
                var hours = seconds / 3600;
                var minutes = (seconds % 3600) / 60;
                seconds = (seconds % 60);
                time = string.Concat(time, String.Format("{0:d2}",hours), ":", String.Format("{0:d2}", minutes), ":", String.Format("{0:d2}", seconds));
                return time;
            }
            return "НЕТ СВЯЗИ С ПЛК";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class FloatToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "0";

            if (value.GetType() != typeof(string))
            {
                return value.ToString();
            }
            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }

    public class IsEqual : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) return false;
            return parameter.ToString() == (value ?? 0).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }

    public class IsLess : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (parameter == null) return false;
                return float.Parse(value.ToString()) < float.Parse(parameter.ToString());
            }
            catch (Exception)
            {

                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }

    public class IsGreater : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (parameter == null) return false;
                if (value == null) return false;
                return float.Parse(value.ToString()) > float.Parse(parameter.ToString());
            }
            catch (Exception)
            {

                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
    public class GetPicturePath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (parameter == null) return @"Pictures\not_founded_image.png";
                if (value == null) return @"Pictures\not_founded_image.png";
                int index = int.Parse(parameter.ToString());
                IEnumerable<StatusDevice> collection = value as IEnumerable<StatusDevice>;
                var arr = collection.Select(s => s.PicturePath).ToArray();
                if (index + 1 <= arr.Length)
                {
                    if (File.Exists(arr[index])) return Path.GetFullPath(arr[index]);
                    return @"Pictures\not_founded_image.png";
                }
                else return @"Pictures\not_founded_image.png";
            }
            catch (Exception)
            {
                
                return @"Pictures\not_founded_image.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        
    }
    public class AccessLevelToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(
            object[] values, Type targetType, object parameter, CultureInfo culture)
        {
           if(values.Length<2) return @"Pictures\not_founded_image.png";
            if (values[0] == null) return @"Pictures\not_founded_image.png";
            if (values[1] == null) return @"Pictures\not_founded_image.png";
            IEnumerable<StatusDevice> collection = values[1] as IEnumerable<StatusDevice>;            
            if (collection == null) return @"Pictures\not_founded_image.png";            
            int index = 0;
            if (int.TryParse(values[0].ToString(), out index))
            {
                var arr = collection.Select(s => s.PicturePath).ToArray();
                if (index + 1 <= arr.Length)
                {
                    if (File.Exists(arr[index])) return Path.GetFullPath(arr[index]);
                    return @"Pictures\not_founded_image.png";
                }
                else return @"Pictures\not_founded_image.png";
            }
            return @"Pictures\not_founded_image.png";
        }

        public object[] ConvertBack(
            object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
