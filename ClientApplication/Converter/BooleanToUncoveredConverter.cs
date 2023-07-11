using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ClientApplication.Converter;

public class BooleanToUncoveredConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isCovered && isCovered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}