using System;
using System.Linq;
using System.Windows.Data;

namespace FilterControls.Framework
{
    public class BoolToStringConverter : BoolToValueConverter<String> { }
    public class BoolToValueConverter<T> : IValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            else
                return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            else
                return value.Equals(TrueValue) ? true : false;
        }
    }
    public class MyMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (values[0] != null && values[1] != null)
                {
                    int count = (int)values[0];
                    int listCount = (int)values[1];
                    if (count == listCount)
                        return true;
                    else
                        return false;
                }
            }
            catch {}
            return false;
        }

        public object[] ConvertBack(object values, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
