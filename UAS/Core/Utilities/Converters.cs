using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Core_AMS.Utilities
{
    public class VisibilityToNullableBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool?)
            {
                return (((bool?)value) == true ? Visibility.Visible : Visibility.Collapsed);
            }
            else if (value is bool)
            {
                return (((bool)value) == true ? Visibility.Visible : Visibility.Collapsed);
            }
            else
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                return (((Visibility)value) == Visibility.Visible);
            }
            else
            {
                return Binding.DoNothing;
            }
        }
    }

    public class ResizeButtonConverter : IValueConverter
    {
        private const int ResizeButtonConverterFactor = 36;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue = value as double?;
            if (doubleValue != null)
            {
                return doubleValue.Value / ResizeButtonConverterFactor;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                return (double)value * ResizeButtonConverterFactor;
            }

            return Binding.DoNothing;
        }
    }

    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue  = (double)value;
            var scale = double.Parse((string)parameter);
            return doubleValue  * scale;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public class YesNoToBooleanConverter : IValueConverter
    {
        private const string YesText = "Yes";
        private const string NoText = "No";
        public static readonly YesNoToBooleanConverter Instance = new YesNoToBooleanConverter();

        private YesNoToBooleanConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(true, value)
                ? YesText
                : NoText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Actually won't be used, but in case you need that
            return Equals(value, YesText);
        }
    }
    public class ApplicationIconGridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            int count = (int)value;
            string type = parameter.ToString();

            if (type == "Row")
            {
                int rowCount = 1;
                if (count > 3)
                {
                    rowCount = count / 3;
                    if (count % 3 > 0)
                        rowCount++;
                }
                return rowCount;
            }
            else if (type == "Col")
            {
                if (count < 4)
                    return count;
                else if (count == 4)
                    return 3;
                else
                    return (int)Math.Ceiling((double)count / (double)2);
            }
            else
                return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
